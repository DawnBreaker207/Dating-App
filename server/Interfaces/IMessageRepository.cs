using server.DTO;
using server.Entities;
using server.Helpers;

namespace server.Interfaces;

public interface IMessageRepository
{
  void AddMessage(Message message);
  void DeleteMessage(Message message);
  Task<Message?> GetMessage(int id);
  Task<PagedList<MessageDto>> GetMessagesForUser();
  Task<IEnumerable<MessageDto>> GetMessageThread(string currentUser, string recipientUsername);
  Task<bool> SaveAllAsync();
}
