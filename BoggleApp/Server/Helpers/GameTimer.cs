using System;
using System.Threading;
using BoggleApp.Server.Hubs;
using BoggleApp.Shared;
using BoggleApp.Shared.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BoggleApp.Server.Helpers
{
    public class GameTimer : IGameTicker
    {
        private readonly IHubContext<GameHub> context;

        private Timer _timer;

        private int remained = 180;

        public GameTimer(IHubContext<GameHub> context)
        {
            this.context = context;
        }

        public void StopCountdown()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            remained = 180;
        }

        public void StartCountdown(Room room)
        {
            if (_timer == null)
                _timer = InitializeTimer(room);

            _timer.Change(0, 1000);

        }

        private Timer InitializeTimer(Room room)
        {
            return new Timer(async (status) =>
            {
                if (remained > 0)
                {
                    await context.Clients.Group(room.Id).SendAsync("TimeLeft", remained.ToString());
                    remained -= 1;
                }
                else
                {
                    StopCountdown();
                }

            }, null, Timeout.Infinite, Timeout.Infinite);
        }
    }
}
