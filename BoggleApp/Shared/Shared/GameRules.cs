using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Shared.Shared
{
    public class Points
    {
        public Points(int score, int minLength, int maxLength)
        {
            Score = score;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public int Score { get; }
        public int MinLength { get; }
        public int MaxLength { get; }
    }


    public class GameRules
    {
        readonly List<Points> points = new List<Points>()
        {
            new Points(0, 1, 2),
            new Points(1, 3, 4),
            new Points(2, 5, 6),
            new Points(3, 7, 7),
            new Points(4, 8, int.MaxValue)
        };

        public GameRules()
        {
        }

        public int GetPoints(string word)
        {
            var length = word.Length;

            if (word.Contains("nj") || word.Contains("lj") || word.Contains("dž"))
                length -= 1;

            return points.Where(p => length >= p.MinLength && length <= p.MaxLength).First().Score;
        }
    }
}
