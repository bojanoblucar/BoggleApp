using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BoggleApp.Client.Shared
{
    public class GameTickerBase : ComponentBase
    {
        protected string time = string.Empty;

        protected string background = "lightgreen";

        [Parameter] public Action OnTimeUp { get; set; }


        public void WriteRemainingTime(int timeRemained)
        {
            TimeSpan times = TimeSpan.FromSeconds(timeRemained);

            time = times.ToString(@"mm\:ss");

            ChangeBackground(timeRemained);

            if (timeRemained == 0)
                OnTimeUp?.Invoke();

            StateHasChanged();
        }

        private void ChangeBackground(int timeRemained)
        {
            if (timeRemained >= 60)
                background = "lightgreen";

            else if (timeRemained > 15 && timeRemained < 60)
                background = "khaki";

            else
                background = "coral";
            
        }
    }
}
