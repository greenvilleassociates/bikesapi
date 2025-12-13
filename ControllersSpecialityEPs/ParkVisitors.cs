using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Mail;
using dirtbike.api.Data;
using dirtbike.api.Models;
using Enterpriseservices;

namespace Enterprise.Controllers
{
    public static class ParksCalendarSpeciality
    {
        public static void MapParkInventoryEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/ParkInventory").WithTags(nameof(Park));
            Globals.ControllerAPIName = "ParksInventoryAPI";
            Globals.ControllerAPINumber = "100";

            group.MapPut("/removeguests/", async (int park, int Removesomeguests) =>
            {
                using (var context = new DirtbikeContext())
                {
                    Park[] someParks = context.Parks.Where(m => m.ParkId == park).ToArray();
                    context.Parks.Attach(someParks[0]);
                    int temp = someParks[0].Currentvisitors;
                    someParks[0].Currentvisitors -= Removesomeguests;
                    await context.SaveChangesAsync();
                    Enterpriseservices.ApiLogger.logapi(Globals.ControllerAPIName, Globals.ControllerAPINumber, "REMOVEGUESTS", 1, "TEST", "TEST");
                    return TypedResults.Accepted($"Updated ParkID: {park} Previous Visitors: {temp} CurrentVisitors: {someParks[0].Currentvisitors}");
                }
            })
            .WithName("RemoveSomeParkGuests")
            .WithOpenApi();

            // ... other endpoints unchanged ...

            group.MapGet("/currentusers/", (int park) =>
            {
                using var context = new DirtbikeContext();

                var parkEntity = context.Parks.FirstOrDefault(m => m.ParkId == park);
                if (parkEntity == null)
                {
                    return "-99";
                }

                if (parkEntity.Currentvisitors == null)
                {
                    parkEntity.Currentvisitors = 0;
                }

                Enterpriseservices.ApiLogger.logapi(
                    Enterpriseservices.Globals.ControllerAPIName,
                    Enterpriseservices.Globals.ControllerAPINumber,
                    "ADDGUESTS_CAPACITY",
                    1,
                    "TEST",
                    "TEST"
                );

                return $"{parkEntity.Maxvisitors} / {parkEntity.Currentvisitors}";
            })
            .WithName("CheckCurrentUsers")
            .WithOpenApi();
        }

        public class ParkCapacityPayload
        {
            public int ParkId { get; set; }
            public string Id { get; set; }
            public int Maxvisitors { get; set; }
            public int Maxcampsites { get; set; }
            public int Currentvisitors { get; set; }
            public int Currentcampsites { get; set; }
            public int Currentadults { get; set; }
            public int Currentchildren { get; set; }
        }
    }
}
