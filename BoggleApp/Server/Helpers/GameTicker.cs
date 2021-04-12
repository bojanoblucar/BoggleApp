using System;
using System.Threading.Tasks;
using BoggleApp.Game.Enums;
using BoggleApp.Game.Setup;
using BoggleApp.Server.Hubs;
using BoggleApp.Shared.Hub;
using Microsoft.AspNetCore.SignalR;

namespace BoggleApp.Server.Helpers
{
    public class GameTicker : IGameTicker
    {
        IHubContext<GameHub> _context;

        private bool _stopCounting = false;

        public GameTicker(IHubContext<GameHub> context)
        {
            _context = context;
        }

        public async void StartCountdown(Room room, int countdownInSeconds)
        {
            _stopCounting = false;
            if (room.GameStatus == RoomStatus.PlayMode)
            {
                //await Task.Delay(500);
                int remained = countdownInSeconds;
                while (remained >= 0 && !_stopCounting)
                {
                    await _context.Clients.Group(room.Id).SendAsync(HubResponses.TimeLeft, remained.ToString());
                    await Task.Delay(1000);
                    
                    remained -= 1;
                }

                
                room.GameStatus = RoomStatus.Initialized;
                //await _context.Clients.Group(room.Id).SendAsync("GameOver", remained.ToString());
            }
        }

        public void StopCountdown()
        {
            _stopCounting = true;
        }
    }
}