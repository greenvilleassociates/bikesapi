using System.Text.Json;
using dirtbike.api.Data;
using dirtbike.api.Models;
using Enterpriseservices; // <-- bring in SessionsLogger

namespace dirtbike.api.Services
{
    public class CGCartService
    {
        public CartProcessingResult CreateCart(CGCompletedCartDto dto)
        {
            using var context = new DirtbikeContext();
            var result = new CartProcessingResult();

            var user = context.Users.FirstOrDefault(u => u.Userid == dto.UserId);
            if (user == null)
            {
                result.OverallResult = "Fail";
                result.Items.Add(new ItemResult
                {
                    ItemNumber = 0,
                    Result = "FailUserNotFound",
                    Message = "User not found"
                });

                // 🔎 Log failure with inbound DTO JSON
                var dtoJson = JsonSerializer.Serialize(dto);
                SessionsLogger.SessionLog(
                    "UnknownUser",
                    dto.UserId,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    "CartService",
                    $"Cart POST failed. Payload={dtoJson}");

                return result;
            }

            int itemIndex = 1;
            bool anyFailures = false;

            foreach (var itemDto in dto.Items)
            {
                var park = context.Parks.FirstOrDefault(p => p.Id == itemDto.Park.Id);
                if (park == null)
                {
                    result.Items.Add(new ItemResult
                    {
                        ItemNumber = itemIndex,
                        Result = "FailParkNotFound",
                        Message = $"Park {itemDto.Park.ParkName} not found"
                    });
                    anyFailures = true;
                }
                else
                {
                    int requestedVisitors = itemDto.NumAdults + itemDto.NumChildren;
                    int existingVisitors = context.Cartitems
                        .Where(ci => ci.Parkid == park.Id &&
                                     ci.ResStart <= itemDto.ResEnd &&
                                     ci.ResEnd >= itemDto.ResStart)
                        .Sum(ci => (ci.Adults ?? 0) + (ci.Children ?? 0));

                    if (existingVisitors + requestedVisitors > park.MaxCapacity)
                    {
                        result.Items.Add(new ItemResult
                        {
                            ItemNumber = itemIndex,
                            Result = "FailCapacity",
                            Message = $"Park {park.ParkName} exceeded capacity for {itemDto.ResStart:d} - {itemDto.ResEnd:d}"
                        });
                        anyFailures = true;
                    }
                    else
                    {
                        result.Items.Add(new ItemResult
                        {
                            ItemNumber = itemIndex,
                            Result = "Success",
                            Message = $"Park {park.ParkName} booking accepted"
                        });
                    }
                }
                itemIndex++;
            }

            // If any failures, reject whole cart
            if (anyFailures)
            {
                result.OverallResult = "Fail";

                // 🔎 Log failure with result JSON
                var resultJson = JsonSerializer.Serialize(result);
                SessionsLogger.SessionLog(
                    user.Username,
                    user.Id,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    "CartService",
                    resultJson);

                return result;
            }

            // ✅ Otherwise, persist Cart, Items, Payment, Booking...
            var cart = new Cart
            {
                Uid = dto.Uid,
                CartId = user.Id,
                Transactiontotal = dto.TransactionTotal,
                Paymentid = dto.PaymentId,
                IsCheckedOut = 1,
                DateAdded = DateOnly.FromDateTime(DateTime.UtcNow),
                Totalcartitems = dto.Items.Count,
                Multipleitems = dto.Items.Count > 1 ? 1 : 0,
                Parkname = dto.Items.Count == 1 ? dto.Items[0].Park.ParkName : null
            };
            context.Carts.Add(cart);
            context.SaveChanges();

            // Add CartItems...
            foreach (var itemDto in dto.Items)
            {
                var item = new Cartitem
                {
                    Cartid = cart.Id,
                    Cartitemdate = DateTime.UtcNow,
                    Itemdescription = itemDto.Park.ParkName,
                    Itemqty = itemDto.NumAdults + itemDto.NumChildren,
                    Itemtotals = itemDto.TotalPrice,
                    Parkid = itemDto.Park.Id,
                    Parkname = itemDto.Park.ParkName,
                    Adults = itemDto.NumAdults,
                    Children = itemDto.NumChildren,
                    NumDays = itemDto.NumDays,
                    ResStart = itemDto.ResStart,
                    ResEnd = itemDto.ResEnd,
                    CreatedDate = DateTime.UtcNow,
                    Userid = dto.UserId
                };
                context.Cartitems.Add(item);
            }
            context.SaveChanges();

            // Add Booking...
            var booking = new Booking
            {
                Uid = dto.Uid,
                Cartid = cart.Id.ToString(),
                TransactionId = dto.PaymentId,
                QuantityAdults = dto.Items.Sum(i => i.NumAdults),
                QuantityChildren = dto.Items.Sum(i => i.NumChildren),
                TotalAmount = dto.TransactionTotal,
                Totalcartitems = dto.Items.Count,
                CartDetailsJson = JsonSerializer.Serialize(dto),
                Reservationstatus = "Confirmed",
                Reservationtype = "Online",
                ResStart = dto.Items.Min(i => i.ResStart),
                ResEnd = dto.Items.Max(i => i.ResEnd),
                NumDays = dto.Items.Sum(i => i.NumDays)
            };
            context.Bookings.Add(booking);
            context.SaveChanges();

            // Add Payment...
            var payment = new Payment
            {
                BookingId = booking.BookingId,
                Userid = user.Id,
                AmountPaid = dto.TransactionTotal,
                TransactionId = dto.PaymentId,
                PaymentDate = DateTime.UtcNow.ToString("o"),
                Transtype = "Sale"
            };
            context.Payments.Add(payment);
            context.SaveChanges();

            result.BookingId = booking.BookingId;
            result.OverallResult = "Success";

            // 🔎 Log success with result JSON
            var successJson = JsonSerializer.Serialize(result);
            SessionsLogger.SessionLog(
                user.Username,
                user.Id,
                DateTime.UtcNow,
                DateTime.UtcNow,
                "CartService",
                successJson);

            return result;
        }
    }
}
