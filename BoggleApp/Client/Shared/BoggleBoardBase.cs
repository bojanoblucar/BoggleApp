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
    public enum RotationAngle
    {
        One = 0,
        Two = 90,
        Three = 180,
        Four = 270
    }

    class Dice
    {
        public string Letter { get; set; }

        public int Rotation { get; set; } = 0;
    }

    public class BoggleBoardBase : ComponentBase
    {
        [Inject] public IJSRuntime Js { get; set; }

        [Parameter] public string RoomId { get; set; }

        [Parameter] public bool DiceRotation { get; set; } = false;

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected string[] letters;

        protected int rotate = 90;

        protected string boardPadding = string.Empty;

        protected double boardOpacity = 1;

        private bool inputFieldInitialized = false;

        private Random rotationRandom;

        private Array rotationValues = Enum.GetValues(typeof(RotationAngle));

        public string[] CurrentShuffle => letters;

        protected override void OnInitialized()
        {
            HubConnection.On<string[], RoomStatus>("ReceiveShuffled", async (message, gameStatus) =>
            {
                if (!inputFieldInitialized)
                {
                    await BoggleJsInterop.InitializeInputField(Js);
                    inputFieldInitialized = true;
                }

                rotationRandom = new Random();           

                boardOpacity = 1;
                letters = message;

                await BoggleJsInterop.SetBoggleBoardValues(Js, letters);

                OnShuffled?.Invoke(gameStatus);

                StateHasChanged();
               
            });
        }

        [Parameter] public Action<RoomStatus> OnShuffled { get; set; }

        public Task Shuffle(bool forceReshuffle = false)
        {     
            return HubConnection.SendAsync("Shuffle", RoomId, forceReshuffle);
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

        public bool IsInBoard(string letter)
        {
            return letters.Contains(letter);
        }

        protected int RotateDice()
        {
            return DiceRotation ? (int)rotationValues.GetValue(rotationRandom.Next(rotationValues.Length)) : 0;
        }
    }
}
