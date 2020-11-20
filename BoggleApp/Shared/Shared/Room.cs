using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BoggleApp.Shared.Enums;
using BoggleApp.Shared.Model;
using BoggleApp.Shared.Shared;

namespace BoggleApp.Shared
{
    public class Room
    {
        private Board _board;

        private readonly IGameTicker _gameTicker;

        public List<User> Users { get; private set; }

        public string[] CurrentSetup { get; set; }

        public Leaderboard Leaderboard { get; private set; }

        public RoomSettings Settings { get; private set; }

        public string Name { get; private set; }

        public string Id { get; private set; }

        public Room(string name, IGameTicker gameTicker, RoomSettings roomSettings = null)
        {
            Name = name;
            _gameTicker = gameTicker;
            Id = Guid.NewGuid().ToString();

            _board = new Board(new GameSetup());
            Users = new List<User>();
            Leaderboard = new Leaderboard();
            Settings = roomSettings ?? new RoomSettings();
        }        

        public void AddUser(User user)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
                Leaderboard.AddUser(user);
            }
        }

        public void RemoveUser(User user)
        {
            if (Users.Contains(user))
            {
                Users.Remove(user);
                Leaderboard.RemoveUser(user);
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
            _gameTicker.StartCountdown(this, Settings.GameDuration);
        }

        private void StopTimer()
        {
            _gameTicker.StopCountdown();
        }

        public IEnumerable<User> GetConnectedUsers()
        {
            return Users.Where(u => u.ConnectionStatus == ConnectionStatus.Connected).ToList();
        }


        public RoomStatus GameStatus { get; set; } = RoomStatus.Initialized;
    }
}
