using dirtbike.api.Data;
using dirtbike.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Enterpriseservices
{
    public class ParkRatingService
    {
        private readonly DirtbikeContext _context;

        public ParkRatingService(DirtbikeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Updates the AverageRating field for a given park based on its reviews.
        /// </summary>
        public void UpdateAverageParkRating(int parkId)
        {
            // Step 1: Calculate average stars directly with LINQ
            var averageRating = _context.ParkReviews
                                        .Where(r => r.ParkId == parkId)
                                        .Select(r => r.Stars)
                                        .DefaultIfEmpty()
                                        .Average();

            // Step 2: Find the park
            var park = _context.Parks
                               .FirstOrDefault(p => p.ParkId == parkId);

            if (park == null)
            {
                throw new InvalidOperationException("Park not found.");
            }

            // Step 3: Update AverageRating
            park.AverageRating = (float)averageRating;

            // Step 4: Save changes
            _context.SaveChanges();

            return;
        }

    }
}