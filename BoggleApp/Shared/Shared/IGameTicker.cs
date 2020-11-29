using System;
using System.Threading;

namespace BoggleApp.Shared.Shared
{
    public interface IGameTicker
    {
        void StopCountdown();
        void StartCountdown(Room room, int countdownInSeconds);
    }
}
