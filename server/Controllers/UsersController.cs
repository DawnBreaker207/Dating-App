using System;
using Server.Data;
using Server.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers;
[ApiController]
// /api/users
[Route("api/[controller]")]
public class UsersController(DataContext context) : BaseAPIController
{
  [AllowAnonymous]
  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
    var users = await context.Users.ToListAsync();

    return users;
  }



  // api/users/id
  [Authorize]
  [HttpGet("{id:int}")]
  public async Task<ActionResult<AppUser>> GetUser(int id)
  {
    var user = await context.Users.FindAsync(id);

    if (user == null) return NotFound();

    return user;
  }
}
