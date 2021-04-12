using BoggleApp.Shared.Hub;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.HubHelpers
{
    public class JoinRoomRequest : IHubRequest<JoinRoomRequestMsg>
    {
        public JoinRoomRequest(string userId, string roomId, bool somethingBool)
        {
            RequestMessage = new JoinRoomRequestMsg()
            {
                UserId = userId,
                RoomId = roomId,
                SomethingBool = somethingBool
            };
        }

        public string RequestName => "JoinRoom";

        public JoinRoomRequestMsg RequestMessage { get; }
    }
}
