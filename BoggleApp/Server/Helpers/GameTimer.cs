using System;
using System.Threading;
using BoggleApp.Game.Enums;
using BoggleApp.Game.Setup;
using BoggleApp.Server.Hubs;
using BoggleApp.Shared.Hub;
using Microsoft.AspNetCore.SignalR;

namespace BoggleApp.Server.Helpers
{
    public class GameTimer : IGameTicker
    {
        private readonly IHubContext<GameHub> context;

        private Timer _timer;

        private int _remained;

        public GameTimer(IHubContext<GameHub> context)
        {
            this.context = context;
        }

        public void StopCountdown()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);         
        }

        public void StartCountdown(Room room, int countdownInSeconds)
        {
            if (_timer == null)
                _timer = InitializeTimer(room);

            _remained = countdownInSeconds;
            _timer.Change(0, 1000);

        }

        private Timer InitializeTimer(Room room)
        {
            return new Timer(async (status) =>
            {
                await context.Clients.Group(room.Id).SendAsync(HubResponses.TimeLeft, _remained.ToString());

                _remained -= 1;

                if (_remained < 0)
                {
                    StopCountdown();
                    room.GameStatus = RoomStatus.Initialized;
                }
                    
 

            }, null, Timeout.Infinite, Timeout.Infinite);
        }
    }
}
