using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Shared.Model
{
    public class Board
    {
        private readonly GameSetup setup;

        public Board(GameSetup setup)
        {
            this.setup = setup;
        }


        public string[] Shuffle()
        {
            var letters = new List<string>();       

            var random = new Random(Guid.NewGuid().GetHashCode());
            foreach(var dice in setup.Shuffle())
            {
                var letter = dice.Flip(random);
                letters.Add(letter);
            }

            return letters.ToArray();          
        }
    }
}
