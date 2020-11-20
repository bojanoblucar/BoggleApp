using System;
namespace BoggleApp.Shared.Shared
{
    public class RoomSettings
    {
        public RoomSettings()
        {
        }

        public bool ShowOnlyBoard { get; set; } = false;

        public bool AutomaticValidation { get; set; } = false;

        public bool RotateWords { get; set; } = false;

        public int GameDuration { get; set; } = 180;
    }
}
