using System;
using System.Collections.Generic;

namespace BoggleApp.Shared.Chat
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
        }

        public List<Message> Messages { get; set; }
    }
}
