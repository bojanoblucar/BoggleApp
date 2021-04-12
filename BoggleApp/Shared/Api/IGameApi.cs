using BoggleApp.Shared.Helpers;
using BoggleApp.Shared.Hub;
using BoggleApp.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoggleApp.Shared.Api
{
    public interface IGameApi
    {
        Task<LoginResponse> Login(string useranme);

        Task JoinRoom(string userId, string roomId, bool somethingBool);

        Task<Result<RoomViewModel>> GetRoom(string userId, string roomid);

        Task<IEnumerable<RoomViewModel>> GetRooms();

        Task<RoomViewModel> CreateRoom(string roomName);

        Task UsersInRoom(string roomId);

        Task AddPointsToUser(string userId, string roomId, int points);

        Task Shuffle(string roomId, bool forceReshuffle);

        void OnJoinRoom(Action<string> action);

        void OnUsersInRoom(Action<IEnumerable<UserViewModel>> action);

        void OnShuffled(Action<ReceiveShuffledResponse> action);

        void OnTimeLeft(Action<string> action);

        bool IsConnected { get; }
    }
}
