using dirtbike.api.Data;
using dirtbike.api.Models;
using dirtbike.api.DTOs;

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
                return result;
            }

            // Otherwise, persist Cart, Items, Payment, Booking
            var cart = new Cart
            {
                Uid = dto.Uid,
                CartId = user.Id,
                Transactiontotal = dto.TransactionTotal,
                Paymentid = dto.PaymentId,
                IsCheckedOut = 1,
                DateAdded = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            context.Carts.Add(cart);
            context.SaveChanges();

            foreach (var itemDto in dto.Items)
            {
                var item = new Cartitem
                {
                    Cartid = cart.Id,
                    Parkid = itemDto.Park.Id,
                    Parkname = itemDto.Park.ParkName,
                    Adults = itemDto.NumAdults,
                    Children = itemDto.NumChildren,
                    Itemtotals = itemDto.TotalPrice,
                    ResStart = itemDto.ResStart,
                    ResEnd = itemDto.ResEnd,
                    CreatedDate = DateTime.UtcNow
                };
                context.Cartitems.Add(item);
            }
            context.SaveChanges();

            var payment = new Payment
            {
                PaymentId = dto.PaymentId,
                UserId = user.Id,
                Amount = dto.TransactionTotal,
                CreatedDate = DateTime.UtcNow
            };
            context.Payments.Add(payment);
            context.SaveChanges();

            var booking = new Booking
            {
                CartId = cart.Id,
                PaymentId = payment.Id,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            };
            context.Bookings.Add(booking);
            context.SaveChanges();

            result.BookingId = booking.Id;
            result.OverallResult = "Success";
            return result;
        }
    }
}
