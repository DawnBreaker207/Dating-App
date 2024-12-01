using System;
using AutoMapper;
using server.DTO;
using server.Extensions;
using Server.Entities;

namespace server.Helpers;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<AppUser, MemberDto>()
    .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
    .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(X => X.IsMain)!.Url));
    ;
    CreateMap<Photo, PhotoDto>();
  }
}
