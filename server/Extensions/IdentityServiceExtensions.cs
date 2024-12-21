using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using server.Entities;
using Server.Data;
using Server.Entities;

namespace server.Extensions;

public static class IdentityServiceExtensions
{
  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddIdentityCore<AppUser>(otp =>
    {
      otp.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddEntityFrameworkStores<DataContext>();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
      {
        var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found");
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    return services;
  }
}
