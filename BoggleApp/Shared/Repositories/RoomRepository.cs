using System;
using System.Collections.Generic;
using System.Linq;
using BoggleApp.Shared.Shared;

namespace BoggleApp.Shared.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private Dictionary<string, Room> _rooms;

        private string _globalRoomGuid;

        public RoomRepository(IGameTicker gameTicker)
        {
            _rooms = new Dictionary<string, Room>();
            InitializeGlobalRoom(gameTicker);
        }

        private void InitializeGlobalRoom(IGameTicker gameTicker)
        {
            var globalRoom = CreateRoom("GlobalRoom", gameTicker);
            _globalRoomGuid = globalRoom.Id;

            //CreateRoom("Another room");
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

        public Room CreateRoom(string roomName, IGameTicker gameTicker)
        {
            Room newRoom = new Room(roomName, gameTicker);
            _rooms.Add(newRoom.Id, newRoom);
            return newRoom;
        }

        public IEnumerable<Room> GetRooms()
        {
            return _rooms.Values.ToList();
        }
    }
}
