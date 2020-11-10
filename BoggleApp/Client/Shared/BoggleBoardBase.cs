using System;
using System.Threading.Tasks;
using BoggleApp.Shared.Enums;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BoggleApp.Client.Shared
{
    public class BoggleBoardBase : ComponentBase
    {
        [Parameter] public string RoomId { get; set; }

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected string[] letters;

        protected string boardPadding = string.Empty;

        protected override void OnInitialized()
        {
            HubConnection.On<string[], RoomStatus>("ReceiveShuffled", (message, gameStatus) =>
            {
                letters = message;
                StateHasChanged();

                    //OnShuffled?.Invoke();
                
                Console.WriteLine(gameStatus);
            });
        }

        public Task Shuffle() => HubConnection.SendAsync("Shuffle", RoomId);

        public void Peek(RoomViewModel room)
        {
            if (room.GameStatus == RoomStatus.PlayMode)
            {
                letters = room.CurrentSetup;
                StateHasChanged();
            }
        }

        protected string[] GetBoardRow(int rowNumber)
        {
            string[] result = new string[4];
            Array.Copy(letters, rowNumber * 4, result, 0, 4);
            return result;
        }

        protected string GetBoardPadding(int rowNumber)
        {
            if (rowNumber == 0)
                return "board-padding-top";
            else if (rowNumber == 3)
                return "board-padding-bottom";
            else
                return string.Empty;
        }

        protected string GetBorderRadius(int rowNumber)
        {
            if (rowNumber == 0)
                return "board-border-radius-top";
            else if (rowNumber == 3)
                return "board-border-radius-bottom";
            else
                return string.Empty;
        }

        //public Action OnShuffled { get; set; }
    }
}
