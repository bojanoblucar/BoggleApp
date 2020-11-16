using System;
namespace BoggleApp.Shared.Chat
{
    public class Message
    {
        public Message(string username, string message)
        {
            Username = username;
            Value = message;
            Time = DateTime.Now;
        }

        public string Username { get; set; }

        public DateTime Time { get; set; }

        public string  Value { get; set; }
    }
}
