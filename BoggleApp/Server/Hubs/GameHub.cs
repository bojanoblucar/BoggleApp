using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AutoMapper;
using BoggleApp.Shared;
using BoggleApp.Shared.Enums;
using BoggleApp.Shared.Repositories;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace BoggleApp.Server.Hubs
{      
    public class GameHub : Hub
    { 
        private readonly IRoomRepository roomRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public GameHub(IRoomRepository roomRepository, IUsersRepository usersRepository, IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }

        public async Task SendMessage(string user, string [] game)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, game);
        }

        public async Task Shuffle(string roomId, bool forceReshuffle)
        {
            var room = roomRepository.GetRoomById(roomId);

            var shuffled = room.ShuffleBoard(forceReshuffle);

            await Clients.Group(roomId).SendAsync("ReceiveShuffled", shuffled, room.GameStatus);
        }

        public async Task JoinRoom(string userId, string roomId, bool isReconnecting)
        {
            User user = usersRepository.GetById(userId);
            if (user.ConnectionId == null)
                user.ConnectionId = Context.ConnectionId;

            var room = roomRepository.GetRoomById(roomId);

            if (!room.HasUser(user.Id))
            {
                user.JoinRoom(room);        
            }

            await AddClientToGroup(room, user.Username);

            await Clients.Caller.SendAsync("OnRoomJoin", room.Id);
        }
        
        public async Task AddPoints(string userId, string roomId, int points)
        {
            var room = roomRepository.GetRoomById(roomId);
            room.Leaderboard.AddScore(userId, points);

            await Clients.Group(roomId).SendAsync("UsersInRoom", GetConnectedPlayers(room));
        }

        public async Task ResetLeaderboard(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            room.Leaderboard.ResetBoard();

            await Clients.Group(roomId).SendAsync("UsersInRoom", GetConnectedPlayers(room));
        }



        public async Task UsersInRoom(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            await Clients.Group(roomId).SendAsync("UsersInRoom", GetConnectedPlayers(room));
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = usersRepository.GetByConnectionId(Context.ConnectionId);
            user.ChangeConnectionStatus();

            foreach (var room in user.JoinedRooms)
            {
                await RemoveClientFromGroup(user, room);          
            }
        } 

        private async Task AddClientToGroup(Room room, string username)
        {
            await Clients.Group(room.Id).SendAsync("UserJoined", $"{username} has joined the room {room.Name}.");

            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

        }


        private async Task RemoveClientFromGroup(User user, Room room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id);

            await Clients.Group(room.Id).SendAsync("UsersInRoom", GetConnectedPlayers(room));
        }


        private IEnumerable<UserViewModel> GetConnectedPlayers(Room room)
        {
            var connectedUsers = room.GetConnectedUsers().Select(u => u.Id).ToList();
            var players = room.Leaderboard.GetBoard(connectedUsers);
            return mapper.Map<IEnumerable<UserViewModel>>(players);
        }
    }
}
