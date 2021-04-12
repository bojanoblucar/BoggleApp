using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Shared.Hub
{
    public class JoinRoomRequestMsg
    {
        public string UserId { get; set; }

        public string RoomId { get; set; }

        public bool SomethingBool { get; set; }
    }
}
