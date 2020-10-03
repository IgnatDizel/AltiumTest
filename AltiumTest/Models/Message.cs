using System;

namespace AltiumTest.Models
{
  public class Message
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string UserName { get; set; }
    public string Text { get; set; }

    public Message(Services.Models.Message message)
    {
      Id = message.Id;
      Created = message.Created;
      UserName = message.UserName;
      Text = message.Text;
    }
  }
}
