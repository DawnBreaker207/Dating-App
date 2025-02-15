using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Helpers;
using server.Interfaces;
using server.Services;
using server.SignalR;
using Server.Data;

namespace server.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddControllers();
    services.AddDbContext<DataContext>(opt =>
    {
      // opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
      opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }
    );
    services.AddCors();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ILikesRepository, LikeRepository>();
    services.AddScoped<IMessageRepository, MessageRepository>();
    services.AddScoped<IPhotoRepository, PhotoRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IPhotoService, PhotoService>();
    services.AddScoped<LogUserActivity>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
    services.AddSignalR();
    services.AddSingleton<PresenceTracker>();

    return services;
  }
}
