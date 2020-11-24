using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;
using BlazorBrowserStorage;
using BoggleApp.Client.Extensions;
using BoggleApp.Client.Interop;
using BoggleApp.Client.Shared;
using BoggleApp.Shared.Enums;
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

        [Inject] public ISessionStorage SessionStorage { get; set; }

        [Inject] public HttpClient Http { get; set; }

        [Inject] public IJSRuntime JSRuntime { get; set; }

        [Parameter] public string RoomId { get; set; }

        protected string username = string.Empty;

        protected bool inputDisabled = true;

        protected UserViewModel user = null;

        protected RoomViewModel room = null;

        protected string InputText = string.Empty;

        protected List<UserViewModel> usersInGroup = new List<UserViewModel>();

        private RoomStatus _roomStatus = RoomStatus.Initialized;

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected BoggleBoard BoggleBoard { get; set; }

        protected GameTicker GameTicker { get; set; }

        public Whiteboard Whiteboard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HubConnection.On<IEnumerable<UserViewModel>>("UsersInRoom", (users) =>
            {
                usersInGroup.Clear();
                usersInGroup.AddRange(users);
                StateHasChanged();
            });

  
            user = await SessionStorage.GetItemModified<UserViewModel>("username");
            username = user?.Username;

            if (user == null || string.IsNullOrEmpty(user?.Username))
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                if (!IsConnected)
                {
                    await HubConnection.SendAsync("JoinRoom", user.Id, RoomId, true);
                }

                var roomResult = await GetRoomAsync(user.Id, RoomId);
                if (roomResult.IsValid)
                {
                    await InitializeRoom(roomResult.Value);
                    StateHasChanged();
                }
                else
                {
                    NavigationManager.NavigateTo("/");
                }                         
            }
        }


        private async Task InitializeRoom(RoomViewModel room)
        {
            if (room.GameStatus == RoomStatus.PlayMode)
            {
                BoggleBoard.Peek(room);
                inputDisabled = false;
                StateHasChanged();
            }

            await UsersInRoom();

            BoggleBoard.OnShuffled = OnShuffled;
            GameTicker.OnTimeUp = OnGameOver;
            Whiteboard.OnScoreChanged = OnScoreChanged;
        }


        private async void OnShuffled(RoomStatus status)
        {
            Whiteboard.Clear();
            inputDisabled = false;
            _roomStatus = status;          

            StateHasChanged();

            await BoggleJsInterop.FocusWordInput(JSRuntime);
        }

        private async void OnGameOver()
        {
            BoggleBoard.GameOver();
            await DisableInput();
            _roomStatus = RoomStatus.Initialized;

            StateHasChanged();
        }

        private async void OnScoreChanged(int newScore, int diff)
        {
            await AddPoints(diff);
            StateHasChanged();
        }


        public async Task Shuffle()
        {
            bool forceReload = _roomStatus == RoomStatus.PlayMode;
            await BoggleBoard.Shuffle(forceReload);         
        }

        public async Task AddPoints(int points)
        {
            await HubConnection.SendAsync("AddPoints", user.Id, room.Id, points);
        }

        public Task UsersInRoom() => HubConnection.SendAsync("UsersInRoom", RoomId);

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;


        public Task<Result<RoomViewModel>> GetRoomAsync(string userId, string roomId)
        {
            return Http.GetAsResult<RoomViewModel>($"game?user={userId}&roomId={roomId}");
        }

        public void OnInputEntered(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {                
                Whiteboard.AddWord(InputText.Trim());
                InputText = string.Empty;
                StateHasChanged();
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
