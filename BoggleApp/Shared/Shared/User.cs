using System;
using System.Collections.Generic;
using BoggleApp.Shared.Enums;

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

        private string _connectionId;
        public string ConnectionId
        {
            get => _connectionId;
            set
            {
                _connectionId = value;
                if (_connectionId != null)
                    ConnectionStatus = ConnectionStatus.Connected;
            }
        }

        public string Id { get; }

        public ConnectionStatus ConnectionStatus { get; set; } = ConnectionStatus.Connected;

        public void JoinRoom(Room room)
        {
            room.AddUser(this);
            JoinedRooms.Add(room);
        }

        public void ChangeConnectionStatus()
        {
            ConnectionStatus = ConnectionStatus == ConnectionStatus.Connected ?
                ConnectionStatus.Disconnected : ConnectionStatus.Connected;

            if (ConnectionStatus == ConnectionStatus.Disconnected)
            {
                ConnectionId = null;
            }
        }
    }
}
