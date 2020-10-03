using System;

namespace AltiumTest.Services.Models
{
  public class NewMessage
  {
    public string UserName { get; set; }
    public string Text { get; set; }

    public NewMessage(string userName, string text)
    {
      if (!IsValidUserName(userName) || !IsValidText(text))
        throw new Exception("invalid message");

      UserName = userName;
      Text = text;
    }

    private bool IsValidText(string text)
    {
      return !string.IsNullOrWhiteSpace(text);
    }

    private bool IsValidUserName(string userName)
    {
      return !string.IsNullOrWhiteSpace(userName);
    }
  }
}
