using System;
using Microsoft.EntityFrameworkCore;
using server.Interfaces;
using server.Services;
using Server.Data;

namespace server.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddControllers();
    services.AddDbContext<DataContext>(opt =>
    {
      opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    }
    );
    services.AddCors();
    services.AddScoped<ITokenService, TokenService>();

    return services;
  }
}
