using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;
using BlazorBrowserStorage;
using BoggleApp.Client.Extensions;
using BoggleApp.Client.HubHelpers;
using BoggleApp.Client.Interop;
using BoggleApp.Client.Shared;
using BoggleApp.Game.Enums;
using BoggleApp.Game.Analysis;
using BoggleApp.Shared.Api;
using BoggleApp.Shared.Helpers;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace BoggleApp.Client.Pages
{
    public class GameRoomBase : ComponentBase
    {
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IJSRuntime JSRuntime { get; set; }

        [Inject] public IGameApi Api { get; set; }

        [Inject] public IUserContext UserContext { get; set; }

        [Parameter] public string RoomId { get; set; }

        protected string username = string.Empty;

        protected bool inputDisabled = true;

        protected UserViewModel user = null;

        protected RoomViewModel room = null;

        protected string InputText = string.Empty;

        protected List<UserViewModel> usersInGroup = new List<UserViewModel>();

        private RoomStatus _roomStatus = RoomStatus.Initialized;

        private BoggleAnalyser _analyser;

        protected BoggleBoard BoggleBoard { get; set; }

        protected GameTicker GameTicker { get; set; }

        public Whiteboard Whiteboard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Api.OnUsersInRoom((users) =>
            {
                usersInGroup.Clear();
                usersInGroup.AddRange(users);
                StateHasChanged();
            });

            Api.OnShuffled(async (onshuffledResponse) => {
                await BoggleBoard.InitializeBoard(onshuffledResponse.Letters);
                OnShuffled(onshuffledResponse.RoomStatus);
            });

            Api.OnTimeLeft((timeRemained) =>
            {
                GameTicker.WriteRemainingTime(int.Parse(timeRemained));
            });

  
            user = await UserContext.GetUser();
            username = user?.Username;

            if (user == null || string.IsNullOrEmpty(user?.Username))
            {
                NavigationManager.NavigateToIndex();
            }
            else
            {
                if (!Api.IsConnected)
                {
                    await Api.JoinRoom(user.Id, room.Id, true);
                }

                var roomResult = await Api.GetRoom(user.Id, RoomId);
                if (roomResult.IsValid)
                {
                    await InitializeRoom(roomResult.Value);
                    StateHasChanged();
                }
                else
                {
                    NavigationManager.NavigateToIndex();
                }                         
            }
        }


        private async Task InitializeRoom(RoomViewModel room)
        {
            this.room = room;
            if (room.GameStatus == RoomStatus.PlayMode)
            {
                BoggleBoard.Peek(room);
                inputDisabled = false;
                StateHasChanged();
            }

            await Api.UsersInRoom(RoomId); //UsersInRoom();
        }

        private async void OnShuffled(RoomStatus status)
        {
            Whiteboard.Clear();
            inputDisabled = false;
            _roomStatus = status;
            _analyser = BoggleAnalyser.CreateForSolution(BoggleBoard.CurrentShuffle);

            StateHasChanged();

            await BoggleJsInterop.ClearWordInput(JSRuntime);
            await BoggleJsInterop.FocusWordInput(JSRuntime);
        }

        #region Child Components Events
        protected async void OnGameOver()
        {
            BoggleBoard.GameOver();
            await DisableInput();
            _roomStatus = RoomStatus.Initialized;

            StateHasChanged();
        }

        protected async void OnScoreChanged(int newScore, int diff)
        {
            await Api.AddPointsToUser(user.Id, room.Id, diff);
            StateHasChanged();
        }
        #endregion


        public async Task Shuffle()
        {
            bool forceReload = _roomStatus == RoomStatus.PlayMode;
            await BoggleBoard.Shuffle(forceReload);         
        }

        public void OnInputEntered(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {              
                if (_analyser.IsSolution(InputText.Trim()))
                {
                    Whiteboard.AddWord(InputText.Trim());
                    InputText = string.Empty;
                    StateHasChanged();
                }
            }
        }

        private async Task DisableInput()
        {
            InputText = string.Empty;
            StateHasChanged();
            await BoggleJsInterop.ClearWordInput(JSRuntime);
            inputDisabled = true;
        }
    }
}
    