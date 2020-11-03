using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorBrowserStorage;
using BoggleApp.Client.Extensions;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BoggleApp.Client.Pages
{
    public class GameRoomBase : ComponentBase
    {
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public ISessionStorage SessionStorage { get; set; }

        [Inject] public HttpClient Http { get; set; }

        [Parameter] public string RoomId { get; set; }

        protected string[] letters;

        protected string message;

        protected string username = string.Empty;

        protected UserViewModel user = null;

        protected RoomViewModel room = null;

        protected List<UserViewModel> usersInGroup = new List<UserViewModel>();

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected override async Task OnInitializedAsync()
        {

            HubConnection.On<string[]>("ReceiveShuffled", (message) =>
            {
                letters = message;
                StateHasChanged();
            });

            HubConnection.On<string>("UserJoined", (msg) =>
            {
                message = msg;
                StateHasChanged();
            });

            HubConnection.On<string>("UserLeft", (msg) =>
            {
                message = msg;
                StateHasChanged();
            });

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
                var response = await GetRoom(user.Id, RoomId);
                if (!response.IsSuccessStatusCode)
                {
                    NavigationManager.NavigateTo("/");
                }

                room = await response.Content.ReadFromJsonAsync<RoomViewModel>();

                await UsersInRoom();
            }
        }

        public Task Shuffle() => HubConnection.SendAsync("Shuffle", RoomId);

        public Task UsersInRoom() => HubConnection.SendAsync("UsersInRoom", RoomId);

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;


        public Task<HttpResponseMessage> GetRoom(string userId, string roomId)
        {
            return Http.GetAsync($"game?user={userId}&roomId={roomId}");
        }

    }
}
