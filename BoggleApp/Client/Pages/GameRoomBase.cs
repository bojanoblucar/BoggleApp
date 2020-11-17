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

        [Parameter] public string RoomId { get; set; }

        protected string message;

        protected string username = string.Empty;

        protected bool inputDisabled = true;

        protected bool shuffleButtonDisabled = false;

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
            /*HubConnection.On<string>("UserJoined", (msg) =>
            {
                message = msg;
                StateHasChanged();
            });

            HubConnection.On<string>("UserLeft", (msg) =>
            {
                message = msg;
                StateHasChanged();
            });*/

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
                
                var response = await GetRoom(user.Id, RoomId);
                if (!response.IsSuccessStatusCode)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    room = await response.Content.ReadFromJsonAsync<RoomViewModel>();

                    if (room.GameStatus == RoomStatus.PlayMode)
                    {
                        BoggleBoard.Peek(room);
                        inputDisabled = false;
                        shuffleButtonDisabled = true;
                        StateHasChanged();
                    }

                    await UsersInRoom();

                    BoggleBoard.OnShuffled = OnShuffled;
                    GameTicker.OnTimeUp = OnGameOver;

                    StateHasChanged();
                }

                
            }
        }


        private void OnShuffled(RoomStatus status)
        {
            Whiteboard.Clear();
            inputDisabled = false;
            //shuffleButtonDisabled = true;
            _roomStatus = status;

            StateHasChanged();
        }

        private void OnGameOver()
        {
            BoggleBoard.GameOver();
            inputDisabled = true;
            shuffleButtonDisabled = false;
            _roomStatus = RoomStatus.Initialized;

            StateHasChanged();
        }


        public async Task Shuffle()
        {
            bool forceReload = _roomStatus == RoomStatus.PlayMode;
            await BoggleBoard.Shuffle(forceReload);         
        }

        public Task UsersInRoom() => HubConnection.SendAsync("UsersInRoom", RoomId);

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;


        public Task<HttpResponseMessage> GetRoom(string userId, string roomId)
        {
            return Http.GetAsync($"game?user={userId}&roomId={roomId}");
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
    }
}
