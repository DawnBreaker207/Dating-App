using Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
  public DbSet<AppUser> Users { get; set; }
}
