using System;
using System.Collections.Generic;

namespace BoggleApp.Shared
{
    public class User
    {
        public List<Room> JoinedRooms { get; private set; }

        public User(string username)
        {
            Username = username;
            Id = Guid.NewGuid().ToString();

            JoinedRooms = new List<Room>();
        }

        public string Username { get; set; }

        public string ConnectionId { get; set; }

        public string Id { get; }

        public void JoinRoom(Room room)
        {
            room.AddUser(this);
            JoinedRooms.Add(room);
        }
    }
}
