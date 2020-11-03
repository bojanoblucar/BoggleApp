using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorBrowserStorage;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using BoggleApp.Client.Extensions;

namespace BoggleApp.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public ISessionStorage SessionStorage { get; set; }

        [Inject] public HttpClient Http { get; set; }

        protected string username;
        protected string newRoom;
        protected string selectedRoom = "-1";
        protected bool alreadyAssigned;
        protected bool isRoomselectionDisabled = false;
        protected bool isJoinRoomButtonDisabled = true;

        public string NewRoom
        {
            get => newRoom;
            set
            {
                newRoom = value;
                isRoomselectionDisabled = !string.IsNullOrEmpty(newRoom);
                OnFormDataChange();
            }
        }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnFormDataChange();
            }
        }

        public string SelectedRoom
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                OnFormDataChange();
            }
        }

        protected UserViewModel user = new UserViewModel();

        protected IEnumerable<RoomViewModel> rooms = new List<RoomViewModel>();

        [CascadingParameter] HubConnection HubConnection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HubConnection.On<string>("OnRoomJoin", (guid) =>
            {
                NavigationManager.NavigateTo($"room/{guid}");
            });


            user = await SessionStorage.GetItemModified<UserViewModel>("username");
            Username = user?.Username;
            alreadyAssigned = user != null;

            await InitializeRooms();
            //selectedRoom = rooms.First().Id;

        }

        protected async Task InitializeRooms()
        {
            rooms = await Http.GetFromJsonAsync<IEnumerable<RoomViewModel>>("game/rooms");
        }

        public async Task JoinGame()
        {
            if (!alreadyAssigned)
            {
                var response = await Http.PostAsJsonAsync("game/user", username);
                if (response.IsSuccessStatusCode)
                {
                    user = await response.Content.ReadFromJsonAsync<UserViewModel>();
                    await SessionStorage.SetItem<UserViewModel>("username", user);
                }

            }

            var room = await GetRoom();
            await JoinRoom(user.Id, room.Id);
        }

        Task JoinRoom(string username, string roomId) => HubConnection.SendAsync("JoinRoom", username, roomId);

        private async Task<RoomViewModel> GetRoom()
        {
            if (selectedRoom == "-1")
            {
                if (!string.IsNullOrEmpty(newRoom))
                {
                    var response = await Http.PostAsJsonAsync("game/create", newRoom);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<RoomViewModel>();
                    }
                }
            }
            else
            {
                return await Task.FromResult(rooms.Where(r => r.Id == selectedRoom).Single());
            }

            return null;
        }


        public void OnFormDataChange()
        {
            isJoinRoomButtonDisabled = string.IsNullOrEmpty(Username) ||
                (SelectedRoom == "-1" && string.IsNullOrEmpty(NewRoom));
            StateHasChanged();
        }
    }
}
