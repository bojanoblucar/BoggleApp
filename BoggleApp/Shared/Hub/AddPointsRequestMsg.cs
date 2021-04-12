using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Shared.Hub
{
    public class AddPointsRequestMsg
    {
        public string UserId { get; set; }

        public string RoomId { get; set; }

        public int Points { get; set; }
    }
}
