using System;

namespace server.DTO;

public class CreateMessageDto
{
  public required string RecipientUserName { get; set; }
  public required string Context { get; set; }
}
