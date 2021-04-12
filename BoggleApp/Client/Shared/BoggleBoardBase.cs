using System;
using System.Threading.Tasks;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;
using Microsoft.JSInterop;
using BoggleApp.Client.Interop;
using BoggleApp.Shared.Api;
using BoggleApp.Game.Enums;

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

        [Inject] public IGameApi Api { get; set; }

        [Parameter] public string RoomId { get; set; }

        [Parameter] public bool DiceRotation { get; set; } = false;

        protected string[] letters;

        protected int rotate = 90;

        protected string boardPadding = string.Empty;

        protected double boardOpacity = 1;

        private bool inputFieldInitialized = false;

        private Random rotationRandom;

        private Array rotationValues = Enum.GetValues(typeof(RotationAngle));

        public string[] CurrentShuffle => letters;


        public async Task InitializeBoard(string [] letters)
        {
            if (!inputFieldInitialized)
            {
                await BoggleJsInterop.InitializeInputField(Js);
                inputFieldInitialized = true;
            }

            rotationRandom = new Random();

            boardOpacity = 1;
            this.letters = letters;

            await BoggleJsInterop.SetBoggleBoardValues(Js, letters);

            StateHasChanged();
        }


        public Task Shuffle(bool forceReshuffle = false)
        {
            return Api.Shuffle(RoomId, forceReshuffle);
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
