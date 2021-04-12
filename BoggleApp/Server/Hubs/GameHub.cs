using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AutoMapper;
using BoggleApp.Game.Setup;
using BoggleApp.Shared.Hub;
using BoggleApp.Shared.Repositories;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        public async Task Shuffle(ShuffleRequestMsg request)
        {
            var room = roomRepository.GetRoomById(request.RoomId);

            var shuffled = room.ShuffleBoard(request.ForceReshuffle);

            var response = new ReceiveShuffledResponse()
            {
                Letters = shuffled,
                RoomStatus = room.GameStatus
            };
            await Clients.Group(request.RoomId).SendAsync(HubResponses.ReceiveShuffled, response);
        }


        public async Task JoinRoom(JoinRoomRequestMsg request)
        {
            User user = usersRepository.GetById(request.UserId);
            if (user.ConnectionId == null)
                user.ConnectionId = Context.ConnectionId;

            var room = roomRepository.GetRoomById(request.RoomId);

            if (!room.HasUser(user.Id))
            {
                user.JoinRoom(room);        
            }

            await AddClientToGroup(room, user.Username);
            await Clients.Caller.SendAsync(HubResponses.OnRoomJoin, room.Id);
        }
        
        public async Task AddPoints(AddPointsRequestMsg request)
        {
            var room = roomRepository.GetRoomById(request.RoomId);
            room.Leaderboard.AddScore(request.UserId, request.Points);

            await Clients.Group(request.RoomId).SendAsync(HubResponses.UsersInRoom, GetConnectedPlayers(room));
        }

        public async Task ResetLeaderboard(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            room.Leaderboard.ResetBoard();

            await Clients.Group(roomId).SendAsync(HubResponses.UsersInRoom, GetConnectedPlayers(room));
        }



        public async Task UsersInRoom(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            await Clients.Group(roomId).SendAsync(HubResponses.UsersInRoom, GetConnectedPlayers(room));
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

            await Clients.Group(room.Id).SendAsync(HubResponses.UsersInRoom, GetConnectedPlayers(room));
        }


        private IEnumerable<UserViewModel> GetConnectedPlayers(Room room)
        {
            var connectedUsers = room.GetConnectedUsers().Select(u => u.Id).ToList();
            var players = room.Leaderboard.GetBoard(connectedUsers);
            return mapper.Map<IEnumerable<UserViewModel>>(players);
        }
    }
}
