using System;
using System.Threading.Tasks;
using BoggleApp.Shared.Enums;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;
using Microsoft.JSInterop;
using BoggleApp.Client.Interop;

namespace BoggleApp.Client.Shared
{
    public class BoggleBoardBase : ComponentBase
    {
        [Inject] public IJSRuntime Js { get; set; }

        [Parameter] public string RoomId { get; set; }

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected string[] letters;

        protected string boardPadding = string.Empty;

        protected double boardOpacity = 1;

        private bool inputFieldInitialized = false;

        protected override void OnInitialized()
        {
            HubConnection.On<string[], RoomStatus>("ReceiveShuffled", async (message, gameStatus) =>
            {
                if (!inputFieldInitialized)
                {
                    await BoggleJsInterop.InitializeInputField(Js);
                    inputFieldInitialized = true;
                }

                boardOpacity = 1;
                letters = message;

                await BoggleJsInterop.SetBoggleBoardValues(Js, letters);

                OnShuffled?.Invoke(gameStatus);

                StateHasChanged();
               
            });
        }

        public Action<RoomStatus> OnShuffled { get; set; }

        public Task Shuffle()
        {     
            return HubConnection.SendAsync("Shuffle", RoomId);
        }

        public void Peek(RoomViewModel room)
        {
            if (room.GameStatus == RoomStatus.PlayMode)
            {
                letters = room.CurrentSetup;
                StateHasChanged();
            }
        }

        public void GameOver()
        {
            boardOpacity = 0.5;
            StateHasChanged();
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

        public bool IsInBoard(string letter)
        {
            return letters.Contains(letter);
        }
    }
}
