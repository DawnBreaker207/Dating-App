using System;
using Server.Data;
using Server.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Controllers;
using Microsoft.AspNetCore.Authorization;
using server.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;
namespace Server.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository) : BaseAPIController
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
  {
    var users = await userRepository.GetMembersAsync();

    return Ok(users);
  }


  // api/users/id
  [HttpGet("{username}")]
  public async Task<ActionResult<MemberDto>> GetUser(string username)
  {
    var user = await userRepository.GetMemberAsync(username);

    if (user == null) return NotFound();

    return user;
  }
}
