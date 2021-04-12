using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Game.Setup
{
    public class Leaderboard
    {
        List<Player> board;

        public Leaderboard()
        {
            board = new List<Player>();
        }

        public void AddUser(User user)
        {
            board.Add(new Player()
            {
                Id = user.Id,
                Username = user.Username,
                Score = 0
            });
        }

        private Player GetPlayer(string id)
        {
            return board.Where(p => p.Id == id).FirstOrDefault();
        }

        public void RemoveUser(User user)
        {
            var player = GetPlayer(user.Id);
            if (player != null)
                board.Remove(player);
        }

        public void AddScore(string userId, int points)
        {
            var player = GetPlayer(userId);
            if (player != null)
                player.Score += points;
        }

        public int GetScore(User user)
        {
            var player = GetPlayer(user.Id);
            return player != null ? player.Score : 0;
        }

        public void ResetBoard()
        {
            board.ForEach(p => p.Score = 0);
        }

        public IEnumerable<Player> GetBoard(List<string> ids = null)
        {
            return ids != null ? board.Where(p => ids.Contains(p.Id)) : board;
        }
    }
}
