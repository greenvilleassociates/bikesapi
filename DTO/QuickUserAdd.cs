using System.ComponentModel.DataAnnotations;

namespace dirtbike.api.DTOs
{
    public class QuickUserAdd
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Activeprofileurl { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]  // ðŸ‘ˆ This makes Swagger show it as required
        public string Plainpassword { get; set; }


        public Models.User ToUser()
        {
            return new Models.User
            {
                Username = this.Username,
                Fullname = this.Fullname,
                Email = this.Email,
                Activeprofileurl = this.Activeprofileurl,
                Activepictureurl = this.Activeprofileurl, // âœ… required non-null
                Role = this.Role,
                Plainpassword = this.Plainpassword
                // Id â†’ auto-increment
                // Userid â†’ trigger assigns
            };
        }
    }
}


