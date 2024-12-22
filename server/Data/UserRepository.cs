using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using server.DTO;
using server.Helpers;
using server.Interfaces;
using Server.Data;
using Server.Entities;

namespace server.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
  public async Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser)
  {
    var query = context.Users
      .Where(x => x.UserName == username)
      .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
      .AsQueryable();

    if (isCurrentUser) query = query.IgnoreQueryFilters();

    return await query.FirstOrDefaultAsync();
  }

  public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
  {
    var query = context.Users.AsQueryable();

    query = query.Where(x => x.UserName != userParams.CurrentUsername);

    if (userParams.Gender != null)
    {
      query = query.Where(x => x.Gender == userParams.Gender);
    }

    var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
    var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

    query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

    query = userParams.OrderBy switch
    {
      "created" => query.OrderByDescending(x => x.Created),
      _ => query.OrderByDescending(x => x.LastActive)
    };
    return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
  }

  public async Task<IEnumerable<AppUser>> GetUserAsync()
  {
    return await context.Users
    .Include(x => x.Photos)
    .ToListAsync();
  }

  public async Task<AppUser?> GetUserByIdAsync(int id)
  {
    return await context.Users.FindAsync(id);
  }

  public async Task<AppUser?> GetUserByPhotoId(int photoId)
  {
    return await context.Users
        .Include(p => p.Photos)
        .IgnoreQueryFilters()
        .Where(P => P.Photos.Any(P => P.Id == photoId))
        .FirstOrDefaultAsync();
  }

  public async Task<AppUser?> GetUserByUserNameAsync(string userName)
  {
    return await context.Users
    .Include(X => X.Photos)
    .SingleOrDefaultAsync(x => x.UserName == userName);
  }

  public void Update(AppUser user)
  {
    context.Entry(user).State = EntityState.Modified;
  }
}
