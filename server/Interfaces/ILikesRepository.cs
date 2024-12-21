using System;
using server.Entities;
using server.Helpers;

namespace server.Interfaces;

public interface ILikesRepository
{
  Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
  Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
  Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
  void DeleteLike(UserLike like);
  void AddLike(UserLike like);
  Task<bool> SaveChange();
}
