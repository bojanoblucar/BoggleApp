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

        //public Action OnShuffled { get; set; }
    }
}
