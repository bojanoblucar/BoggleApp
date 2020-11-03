using System;
using System.Collections.Generic;
using System.Linq;
using BoggleApp.Shared.Model;

namespace BoggleApp.Shared
{
    public class Room
    {
        private Board _board;

        public List<User> Users { get; private set; }

        public Room(string name)
        {
            Name = name;
            _guid = Guid.NewGuid();

            _board = new Board(new GameSetup());
            Users = new List<User>();
        }

        private Guid _guid;

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

        public string[] ShuffleBoard()
        {
            var newSetup = _board.Shuffle();
            Notify(newSetup);
            return newSetup;
        }

        public void Notify(string [] newSetup)
        {
            //notify each user of new board gameplay
        }
    }
}
