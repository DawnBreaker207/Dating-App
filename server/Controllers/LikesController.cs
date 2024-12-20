using System;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Entities;
using server.Extensions;
using server.Interfaces;

namespace server.Controllers;

public class LikesController(ILikesRepository likesRepository) : BaseAPIController
{
  [HttpPost("{targetUserId:int}")]
  public async Task<ActionResult> ToggleLike(int targetUserId)
  {
    var sourceUserId = User.GetUserId();

    if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

    var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);

    if (existingLike == null)
    {
      var like = new UserLike
      {
        SourceUserId = sourceUserId,
        TargetUserId = targetUserId
      };
      likesRepository.AddLike(like);
    }
    else
    {
      likesRepository.DeleteLike(existingLike);
    }
    if (await likesRepository.SaveChange()) return Ok();

    return BadRequest("Failed to update like");
  }

  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLiekIds()
  {
    return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate)
  {
    var users = await likesRepository.GetUserLikes(predicate, User.GetUserId());

    return Ok(users);
  }
}
