using System;
using System.Collections.Generic;
using BoggleApp.Shared.Shared;

namespace BoggleApp.Shared.Repositories
{
    public interface IRoomRepository
    {
        Room CreateRoom(string roomName, IGameTicker gameTicker);
        Room GetGlobalRoom();
        Room GetRoomById(string guid);
        IEnumerable<Room> GetRooms();
        void RemoveEmptyRooms();
    }
}