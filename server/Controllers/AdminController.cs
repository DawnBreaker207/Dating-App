using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class AdminController : BaseAPIController
{
  [Authorize(Policy = "RequiredAdminRole")]
  [HttpGet("users-with-roles")]
  public ActionResult GetUsersWithRoles()
  {
    return Ok("Only admins can see this");
  }
  [Authorize(Policy = "ModeratePhotoRole")]
  [HttpGet("photos-to-moderate")]
  public ActionResult GetPhotosForModeration()
  {
    return Ok("Admins or moderators can see this");
  }
}
