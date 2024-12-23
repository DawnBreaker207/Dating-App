using server.DTO;
using server.Helpers;
using Server.Entities;

namespace server.Interfaces;

public interface IUserRepository
{
  void Update(AppUser user);
  Task<IEnumerable<AppUser>> GetUsersAsync();
  Task<AppUser?> GetUserByIdAsync(int id);
  Task<AppUser?> GetUserByUserNameAsync(string userName);
  Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
  Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser);
  Task<AppUser?> GetUserByPhotoId(int photoId);

}
