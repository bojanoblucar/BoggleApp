using System;
using BoggleApp.Shared.Enums;

namespace BoggleApp.Shared.ViewModels
{
    public class RoomViewModel
    {
        public RoomViewModel()
        {
        }

        public string Name { get; set; }

        public string Id { get; set; }

        public RoomStatus GameStatus { get; set; }

        public string [] CurrentSetup { get; set; }
    }
}
