using Microsoft.AspNetCore.Mvc;
using server.Controllers;
using Microsoft.AspNetCore.Authorization;
using server.Interfaces;
using AutoMapper;
using server.DTO;
using server.Extensions;
using Server.Entities;
using server.Helpers;
namespace Server.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseAPIController
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
  {
    userParams.CurrentUsername = User.GetUserName();
    var users = await userRepository.GetMembersAsync(userParams);

    Response.AddPaginationHeader(users);

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

  [HttpPut]
  public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
  {
    var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());

    if (user == null) return BadRequest("Could not find user");

    mapper.Map(memberUpdateDto, user);

    if (await userRepository.SaveAllSync()) return NoContent();

    return BadRequest("Failed to update the user");
  }
  [HttpPost("add-photo")]
  public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
  {
    var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());

    if (user == null) return BadRequest("Cannot update user");

    var result = await photoService.AddPhotoAsync(file);

    if (result.Error != null) return BadRequest(result.Error.Message);

    var photo = new Photo
    {
      Url = result.SecureUrl.AbsoluteUri,
      PublicId = result.PublicId
    };

    user.Photos.Add(photo);

    if (await userRepository.SaveAllSync())
      return CreatedAtAction(nameof(GetUser),
              new { username = user.UserName },
              mapper.Map<PhotoDto>(photo));

    return BadRequest("Problem adding photo");
  }

  [HttpPut("set-main-photo/{photoId:int}")]
  public async Task<ActionResult> SetMainPhoto(int photoId)
  {
    var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());

    if (user == null) return BadRequest("Could not find user");

    var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

    if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

    var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

    if (currentMain != null) currentMain.IsMain = false;
    photo.IsMain = true;
    if (await userRepository.SaveAllSync()) return NoContent();

    return BadRequest("Problem setting main photo");
  }

  [HttpDelete("delete-photo/{photoId}")]
  public async Task<ActionResult> DeletePhoto(int photoId)
  {
    var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());

    if (user == null) return BadRequest("User not found");

    var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

    if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

    if (photo.PublicId != null)
    {
      var result = await photoService.DeletePhotoAsync(photo.PublicId);
      if (result.Error != null) return BadRequest(result.Error.Message);
    }

    user.Photos.Remove(photo);

    if (await userRepository.SaveAllSync()) return Ok();

    return BadRequest("Problem deleting photo");
  }
}
