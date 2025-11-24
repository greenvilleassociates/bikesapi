using dirtbike.api.Models;
using dirtbike.api.Data;
using Microsoft.EntityFrameworkCore;
namespace Enterpriseservices;

public class MyPasswords
{
    public static async Task HashAllUserPasswordsAsync()
    {
            using (var context = new DirtbikeContext())
            {
                // Clear out any "string" placeholders
                var badUsers = context.Users.Where(u => u.Hashedpassword == "string").ToList();
                foreach (var u in badUsers)
                {
                    u.Hashedpassword = null;
                }
                context.SaveChanges();

                // Now loop through and hash
                var users = context.Users.ToList();
                foreach (var user in users)
                {
                    if (!string.IsNullOrEmpty(user.Plainpassword) && string.IsNullOrEmpty(user.Hashedpassword))
                    {
                        user.Hashedpassword = BCrypt.Net.BCrypt.HashPassword(user.Plainpassword);
                    }
                }

                context.SaveChanges();

                // Verification
                var updatedUsers = context.Users
                    .Select(u => new { u.Id, u.Plainpassword, u.Hashedpassword })
                    .ToList();

                foreach (var u in updatedUsers)
                {
                    Console.WriteLine($"Id: {u.Id}, Plain: {u.Plainpassword}, Hashed: {u.Hashedpassword}");
                }
            }
    }
}
