using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Shared.Hub
{
    public static class HubResponses
    {
        public static string OnRoomJoin => "OnRoomJoin";
        public static string UsersInRoom => "UsersInRoom";
        public static string ReceiveShuffled => "ReceiveShuffled";
        public static string TimeLeft => "TimeLeft";
    }
}
