using System;

namespace AltiumTest.Data.Entities
{
  public class Message: IEntity
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string UserName { get; set; }
    public string Text { get; set; }
  }
}
