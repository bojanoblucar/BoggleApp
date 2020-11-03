using System;
using System.Collections.Generic;

namespace BoggleApp.Shared.Repositories
{
    public interface IRoomRepository
    {
        Room CreateRoom(string roomName);
        Room GetGlobalRoom();
        Room GetRoomById(string guid);
        IEnumerable<Room> GetRooms();
    }
}