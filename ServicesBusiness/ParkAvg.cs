using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dirtbike.api.Data;
using System.Text;
using dirtbike.api.Models;


namespace enterpriseservices
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
        public async Task UpdateAverageParkRatingAsync(int parkId)
        {
            // Step 1: Calculate average stars directly with LINQ
            var averageRating = await _context.ParkReviews
                                              .Where(r => r.ParkId == parkId)
                                              .Select(r => r.Stars)
                                              .DefaultIfEmpty()   // prevents exception if no reviews
                                              .AverageAsync();

            // Step 2: Find the park
            var park = await _context.Parks
                                     .FirstOrDefaultAsync(p => p.ParkId == parkId);

            if (park == null)
            {
                throw new InvalidOperationException("Park not found.");
            }

            // Step 3: Update AverageRating
            park.AverageRating = (float)averageRating;

            // Step 4: Save changes
            await _context.SaveChangesAsync();
        }
    }
}