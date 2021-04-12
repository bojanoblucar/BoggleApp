using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.HubHelpers
{
    public class UsersInRoomRequest : IHubRequest<string>
    {
        public UsersInRoomRequest(string roomId)
        {
            RequestMessage = roomId;
        }

        public string RequestName => "UsersInRoom";

        public string RequestMessage { get; }
    }
}
