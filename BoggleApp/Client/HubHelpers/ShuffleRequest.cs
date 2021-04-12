using BoggleApp.Shared.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.HubHelpers
{
    public class ShuffleRequest : IHubRequest<ShuffleRequestMsg>
    {
        public ShuffleRequest(string roomId, bool forceReshuffle)
        {
            RequestMessage = new ShuffleRequestMsg()
            {
                RoomId = roomId,
                ForceReshuffle = forceReshuffle
            };
        }

        public string RequestName => "Shuffle";

        public ShuffleRequestMsg RequestMessage { get; }
    }
}
