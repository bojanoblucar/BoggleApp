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

            var currentStatus = room.GameStatus;
            var shuffled = room.ShuffleBoard(forceReshuffle);            

            if (currentStatus == RoomStatus.Initialized)
            {
                await Clients.Group(roomId).SendAsync("ReceiveShuffled", shuffled, room.GameStatus);

            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveShuffled", shuffled, room.GameStatus);
            }
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


        public async Task UsersInRoom(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            await Clients.Group(roomId).SendAsync("UsersInRoom", mapper.Map<IEnumerable<UserViewModel>>(room.GetConnectedUsers()));
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = usersRepository.GetByConnectionId(Context.ConnectionId);
            user.ChangeConnectionStatus();

            foreach (var room in user.JoinedRooms)
            {
                //room.RemoveUser(user);
                await RemoveClientFromGroup(user, room);          
            }

            //usersRepository.RemoveUser(Context.ConnectionId);
        } 

        private async Task AddClientToGroup(Room room, string username)
        {
            await Clients.Group(room.Id).SendAsync("UserJoined", $"{username} has joined the room {room.Name}.");

            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

        }


        private async Task RemoveClientFromGroup(User user, Room room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id);

            await Clients.Group(room.Id).SendAsync("UsersInRoom", mapper.Map<IEnumerable<UserViewModel>>(room.GetConnectedUsers()));
        }
    }
}
