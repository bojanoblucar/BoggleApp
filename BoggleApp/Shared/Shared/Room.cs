using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using BoggleApp.Shared.Enums;
using BoggleApp.Shared.Model;
using BoggleApp.Shared.Shared;

namespace BoggleApp.Shared
{
    public class Room
    {
        private Board _board;

        private int _gameCountdown = 180;

        public List<User> Users { get; private set; }

        public string[] CurrentSetup { get; set; }

        public Room(string name, IGameTicker gameTicker)
        {
            Name = name;
            this.gameTicker = gameTicker;
            _guid = Guid.NewGuid();

            _board = new Board(new GameSetup());
            Users = new List<User>();   
        }

        private Guid _guid;
        private readonly IGameTicker gameTicker;
        

        public string Id => _guid.ToString();
        public string Name { get; }

        public void AddUser(User user)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
            }
        }

        public void RemoveUser(User user)
        {
            if (Users.Contains(user))
            {
                Users.Remove(user);
            }
        }

        public bool HasUser(string userId)
        {
            return Users.Any(u => u.Id == userId);
        }

        public string[] ShuffleBoard(bool forceResuffle = false)
        {
            if (forceResuffle && GameStatus == RoomStatus.PlayMode)
            {
                GameStatus = RoomStatus.Initialized;
                StopTimer();
            }

            if (GameStatus == RoomStatus.Initialized)
            {
                CurrentSetup = _board.Shuffle();
                GameStatus = RoomStatus.PlayMode;
                StartTimer();
            }
                           
            return CurrentSetup;
        }

        private void StartTimer()
        {
            gameTicker.StartCountdown(this, _gameCountdown);
        }

        private void StopTimer()
        {
            gameTicker.StopCountdown();
        }

        public IEnumerable<User> GetConnectedUsers()
        {
            return Users.Where(u => u.ConnectionStatus == ConnectionStatus.Connected).ToList();
        }


        public RoomStatus GameStatus { get; set; } = RoomStatus.Initialized;
    }
}
