using BoggleApp.Client.Extensions;
using BoggleApp.Client.HubHelpers;
using BoggleApp.Shared.Api;
using BoggleApp.Shared.Helpers;
using BoggleApp.Shared.Hub;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BoggleApp.Client.Api
{
    public class GameApi : IGameApi
    {
        private readonly HttpClient httpClient;
        private readonly HubCommunicator hub;
        private readonly IUserContext userContext;

        public GameApi(HttpClient httpClient, HubCommunicator hub, IUserContext userContext)
        {
            this.httpClient = httpClient;
            this.hub = hub;
            this.userContext = userContext;
        }

        public bool IsConnected => hub.HubConnection != null && hub.HubConnection.State == HubConnectionState.Connected;

        public Task AddPointsToUser(string userId, string roomId, int points)
        {
            return hub.SendAsync(new AddPointsRequest(userId, roomId, points));
        }

        public async Task<RoomViewModel> CreateRoom(string roomName)
        {
            var response = await httpClient.PostAsJsonWithResultAsync<RoomViewModel, string>("game/create", roomName);
            if (response.IsValid)
                return response.Value;

            return null;
        }

        public Task<Result<RoomViewModel>> GetRoom(string userId, string roomId)
        {
            return httpClient.GetAsResult<RoomViewModel>($"game?user={userId}&roomId={roomId}");
        }

        public Task<IEnumerable<RoomViewModel>> GetRooms()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<RoomViewModel>>("game/rooms");
        }

        public async Task JoinRoom(string userId, string roomId, bool somethingBool)
        {
            await hub.SendAsync(new JoinRoomRequest(userId, roomId, somethingBool));
        }

        public async Task<LoginResponse> Login(string username)
        {
            var response = await httpClient.PostAsJsonWithResultAsync<LoginResponse, string>("game/user", username);
            if (response.IsValid)
            {
                userContext.SetUser(response.Value.User);
                userContext.SetToken(response.Value.Token);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Value.Token);
                await hub.SetupAuthenticated(response.Value.Token);

                return response.Value;
            }

            return null;
        }

        public void OnJoinRoom(Action<string> action)
        {
            hub.On(HubResponses.OnRoomJoin, action);
        }

        public void OnShuffled(Action<ReceiveShuffledResponse> action)
        {
            hub.On(HubResponses.ReceiveShuffled, action);
        }

        public void OnTimeLeft(Action<string> action)
        {
            hub.On(HubResponses.TimeLeft, action);
        }

        public void OnUsersInRoom(Action<IEnumerable<UserViewModel>> action)
        {
            hub.On(HubResponses.UsersInRoom, action);
        }

        public Task Shuffle(string roomId, bool forceReshuffle)
        {
            return hub.SendAsync(new ShuffleRequest(roomId, forceReshuffle));
        }

        public Task UsersInRoom(string roomId)
        {
            return hub.SendAsync(new UsersInRoomRequest(roomId));
        }
    }
}
