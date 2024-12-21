using Server.Entities;

namespace server.Interfaces;

public interface ITokenService
{
  Task<string> CreateToken(AppUser user);
}
