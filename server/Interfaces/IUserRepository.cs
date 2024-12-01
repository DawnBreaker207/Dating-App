using System;
using Server.Entities;

namespace server.Interfaces;

public interface IUserRepository
{
  void Update(AppUser user);
  Task<bool> SaveAllSync();
  Task<IEnumerable<AppUser>> GetUserAsync();
  Task<AppUser?> GetUserByIdAsync(int id);
  Task<AppUser?> GetUserByUserNameAsync(string userName);
  Task<IEnumerable<MemberDto>> GetMembersAsync();
  Task<MemberDto?> GetMemberAsync(string user);

}
