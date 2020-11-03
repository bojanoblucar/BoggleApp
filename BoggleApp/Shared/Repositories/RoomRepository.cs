using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Shared.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private Dictionary<string, Room> _rooms;

        private string _globalRoomGuid;

        public RoomRepository()
        {
            _rooms = new Dictionary<string, Room>();
            InitializeGlobalRoom();
        }

        private void InitializeGlobalRoom()
        {
            var globalRoom = CreateRoom("GlobalRoom");
            _globalRoomGuid = globalRoom.Id;

            CreateRoom("Another room");
 
        }

        public Room GetRoomById(string id)
        {
            if (!_rooms.ContainsKey(id))
                throw new ArgumentException($"No room with ID={id}");
            return _rooms[id];
        }

        public Room GetGlobalRoom()
        {
            return GetRoomById(_globalRoomGuid);
        }

        public Room CreateRoom(string roomName)
        {
            Room newRoom = new Room(roomName);
            _rooms.Add(newRoom.Id, newRoom);
            return newRoom;
        }

        public IEnumerable<Room> GetRooms()
        {
            return _rooms.Values.ToList();
        }
    }
}
