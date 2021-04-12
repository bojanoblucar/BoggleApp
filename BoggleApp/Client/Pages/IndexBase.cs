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
using System.Net.Http.Headers;
using BoggleApp.Client.HubHelpers;
using BoggleApp.Shared.Api;
using BoggleApp.Client.Services;
using BoggleApp.Shared.Helpers;

namespace BoggleApp.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IGameApi Api { get; set; }

        [Inject] public IUserContext UserContext { get; set; }

        protected string username;
        protected string newRoom;
        protected string selectedRoom = "-1";
        protected bool alreadyAssigned;
        protected bool isRoomselectionDisabled = false;
        protected bool isJoinRoomButtonDisabled = true;

        protected bool AlreadyAssigned => user != null;

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

        protected override async Task OnInitializedAsync()
        {
            user = await UserContext.GetUser();
            Username = user?.Username;

            await InitializeRooms();
        }

        public async Task InitializeRooms()
        {
            rooms = await Api.GetRooms();
        }

        public async Task JoinGame()
        {
            if (!AlreadyAssigned)
            {   
                var response = await Api.Login(username);
                user = await UserContext.GetUser();

                Api.OnJoinRoom((roomId) => NavigationManager.NavigateToRoom(roomId));
            }

            var room = await GetRoom();
            await Api.JoinRoom(user.Id, room.Id, false);
        }

        private async Task<RoomViewModel> GetRoom()
        {
            if (selectedRoom == "-1" && !string.IsNullOrEmpty(newRoom))
            {
                return await Api.CreateRoom(newRoom);            
            }
            else
            {
                return await Task.FromResult(rooms.Where(r => r.Id == selectedRoom).Single());
            }
        }


        public void OnFormDataChange()
        {
            isJoinRoomButtonDisabled = string.IsNullOrEmpty(Username) ||
                (SelectedRoom == "-1" && string.IsNullOrEmpty(NewRoom));
            StateHasChanged();
        }
    }
}
