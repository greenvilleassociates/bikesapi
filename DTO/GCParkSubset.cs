using System;
using System.Collections.Generic;
using System.Linq;
using dirtbike.api.Models;

namespace dirtbike.api.DTOs
{
    public class CGPARKS
    {
        public required string Id { get; set; }              // UUID exposed to UI
        public required string ParkName { get; set; }        // Maps from Park.Name
        public required string Location { get; set; }        // Maps from Park.Address
        public required string Description { get; set; }     // Maps from Park.Description
        public required double AdultPrice { get; set; }      // Maps from Park.AdultPrice
        public required double ChildPrice { get; set; }      // Maps from Park.ChildPrice
        public required string ImageUrl { get; set; }        // Maps from Park.Pic1url
        public required List<ParkReviewDto> Reviews { get; set; } = new List<ParkReviewDto>();

        /// <summary>
        /// Factory method to map from Park + Reviews(+Users) → CGPARKS DTO
        /// </summary>
        public static CGPARKS FromPark(Park park, IEnumerable<ReviewWithUser> reviewsWithUsers)
        {
            return new CGPARKS
            {
                Id = park.Id ?? string.Empty,
                ParkName = park.Name ?? string.Empty,
                Location = park.Address ?? string.Empty,
                Description = park.Description ?? string.Empty,
                AdultPrice = park.AdultPrice ?? 0,
                ChildPrice = park.ChildPrice ?? 0,
                ImageUrl = park.Pic1url ?? string.Empty,
                Reviews = reviewsWithUsers.Select(r => new ParkReviewDto
                {
                    Author = new AuthorDto
                    {
                        Id = r.User.Uidstring ?? r.Review.Useridasstring ?? string.Empty,
                        DisplayName = r.User.Displayname ?? string.Empty,
                        FullName = r.User.Fullname ?? string.Empty,
                        DateOfBirth = r.User.DateOfBirth
                    },
                    Rating = r.Review.Stars,
                    DateWritten = r.Review.DatePosted,
                    DateVisited = r.Review.DateApproved, // adjust if another field better represents "visited"
                    Review = r.Review.Description
                }).ToList()
            };
        }
    }

    public class ParkReviewDto
    {
        public AuthorDto Author { get; set; } = new AuthorDto();
        public int Rating { get; set; }
        public string DateWritten { get; set; } = string.Empty;
        public string DateVisited { get; set; } = string.Empty;
        public string Review { get; set; } = string.Empty;
    }

    public class AuthorDto
    {
        public string Id { get; set; } = string.Empty;          // UserIdAsString or Uidstring
        public string DisplayName { get; set; } = string.Empty; // User.Displayname
        public string FullName { get; set; } = string.Empty;    // User.Fullname
        public DateOnly? DateOfBirth { get; set; }              // User.DateOfBirth
    }

    public class ReviewWithUser
    {
        public ParkReview Review { get; set; }
        public User User { get; set; }
    }
}

