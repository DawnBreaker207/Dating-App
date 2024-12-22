using server.DTO;
using Server.Entities;

namespace server.Interfaces;

public interface IPhotoRepository
{
  Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
  Task<Photo?> GetPhotoById(int id);
  void RemovePhoto(Photo photo);
}
