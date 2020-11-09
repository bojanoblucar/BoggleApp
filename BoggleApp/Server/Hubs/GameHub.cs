using System;
using System.Collections.Generic;
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

        public async Task Shuffle(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);

            var shuffled = room.ShuffleBoard();            

            if (room.GameStatus == RoomStatus.Initialized)
            {
                await Clients.Group(roomId).SendAsync("ReceiveShuffled", shuffled, room.GameStatus);

            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveShuffled", shuffled, room.GameStatus);
            }
        }

        public async Task JoinRoom(string userId, string roomId)
        {
            User user = usersRepository.GetById(userId);
            if (user.ConnectionId == null)
                user.ConnectionId = Context.ConnectionId;

            var room = roomRepository.GetRoomById(roomId);

            if (!room.HasUser(user.Id))
            {
                user.JoinRoom(room);

                await AddToGroup(room, user.Username);
            }

            await Clients.Caller.SendAsync("OnRoomJoin", room.Id);
        }


        public async Task UsersInRoom(string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            await Clients.Group(roomId).SendAsync("UsersInRoom", mapper.Map<IEnumerable<UserViewModel>>(room.Users));
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = usersRepository.GetByConnectionId(Context.ConnectionId);
            foreach (var room in user.JoinedRooms)
            {
                room.RemoveUser(user);
                await RemoveFromGroup(user, room);          
            }

            usersRepository.RemoveUser(Context.ConnectionId);
        }

        private async Task AddToGroup(Room room, string username)
        {
            await Clients.Group(room.Id).SendAsync("UserJoined", $"{username} has joined the room {room.Name}.");

            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

        }


        private async Task RemoveFromGroup(User user, Room room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id);

            await Clients.Group(room.Id).SendAsync("UserLeft", $"{user.Username} has left the room.");

            await Clients.Group(room.Id).SendAsync("UsersInRoom", mapper.Map<IEnumerable<UserViewModel>>(room.Users));
        }

        public async Task Countdown(string roomId)
        {
            int remained = 3 * 60;
            while(remained >= 0)
            {                
                await Task.Delay(1000);
  
                Console.WriteLine(remained);
                await Clients.Group(roomId).SendAsync("CountdownEllapsed", remained.ToString());
                remained -= 1;
            }
            
        }


        public void CountDownElapsed(int remained, string roomId)
        {
            Console.WriteLine(remained);
            Clients.Group(roomId).SendAsync("CountdownEllapsed", remained.ToString());
        }
    }
}
