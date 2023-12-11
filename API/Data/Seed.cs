

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {

            if (context.AppUsers.Any()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            foreach (var user in users)
            {

                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.AppUsers.Add(user);

            }
            await context.SaveChangesAsync();

        }
    }
}