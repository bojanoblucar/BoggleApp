using BoggleApp.Shared.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.HubHelpers
{
    public class AddPointsRequest : IHubRequest<AddPointsRequestMsg>
    {
        public AddPointsRequest(string userId, string roomId, int points)
        {
            RequestMessage = new AddPointsRequestMsg()
            {
                UserId = userId,
                RoomId = roomId,
                Points = points
            };
        }

        public string RequestName => "AddPoints";

        public AddPointsRequestMsg RequestMessage { get; }
    }
}
