using server.DTO;
using server.Helpers;
using Server.Entities;

namespace server.Interfaces;

public interface IUserRepository
{
  void Update(AppUser user);
  Task<IEnumerable<AppUser>> GetUserAsync();
  Task<AppUser?> GetUserByIdAsync(int id);
  Task<AppUser?> GetUserByUserNameAsync(string userName);
  Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
  Task<MemberDto?> GetMemberAsync(string user, bool isCurrentUser);
  Task<AppUser?> GetUserByPhotoId(int photoId);

}
