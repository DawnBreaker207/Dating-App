using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entities;

namespace server.Data;

public class Seed
{
  public static async Task SeedUsers(DataContext context)
  {
    if (await context.Users.AnyAsync()) return;


    var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

    if (users == null) return;

    foreach (var user in users)
    {
      using var hmac = new HMACSHA512();

      context.Users.Add(user);
    }

    await context.SaveChangesAsync();
  }
}
