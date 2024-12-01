using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using server.Interfaces;
using Server.Data;
using Server.Entities;

namespace server.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
  public async Task<MemberDto?> GetMemberAsync(string username)
  {
    return await context.Users
      .Where(x => x.UserName == username)
    .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
    .SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<MemberDto>> GetMembersAsync()
  {
    return await context.Users.ProjectTo<MemberDto>(mapper.ConfigurationProvider).ToListAsync();
  }

  public async Task<IEnumerable<AppUser>> GetUserAsync()
  {
    return await context.Users
    .Include(X => X.Photos)
    .ToListAsync();
  }

  public async Task<AppUser?> GetUserByIdAsync(int id)
  {
    return await context.Users.FindAsync(id);
  }

  public async Task<AppUser?> GetUserByUserNameAsync(string userName)
  {
    return await context.Users
    .Include(X => X.Photos)
    .SingleOrDefaultAsync(x => x.UserName == userName);
  }

  public async Task<bool> SaveAllSync()
  {
    return await context.SaveChangesAsync() > 0;
  }

  public void Update(AppUser user)
  {
    context.Entry(user).State = EntityState.Modified;
  }
}
