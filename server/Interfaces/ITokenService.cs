using System;
using Server.Entities;

namespace server.Interfaces;

public interface ITokenService
{
  string CreateToken(AppUser user);
}
