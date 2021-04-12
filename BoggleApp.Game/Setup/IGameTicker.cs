using System;
using System.Threading;

namespace BoggleApp.Game.Setup
{
    public interface IGameTicker
    {
        void StopCountdown();
        void StartCountdown(Room room, int countdownInSeconds);
    }
}
