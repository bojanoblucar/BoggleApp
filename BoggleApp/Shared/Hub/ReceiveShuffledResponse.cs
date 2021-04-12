using BoggleApp.Game.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Shared.Hub
{
    public class ReceiveShuffledResponse
    {
        public string[] Letters { get; set; }

        public RoomStatus RoomStatus { get; set; }
    }
}
