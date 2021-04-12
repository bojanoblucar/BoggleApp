using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Shared.Hub
{
    public class ShuffleRequestMsg
    {
        public string RoomId { get; set; }

        public bool ForceReshuffle { get; set; }
    }
}
