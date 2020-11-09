using System;
namespace BoggleApp.Shared.Model
{
    public class Game
    {
        private Board _board;

        public Game()
        {
            _board = new Board(new GameSetup());
        }

        public string [] StartNewGame()
        {
            var newSetup = _board.Shuffle();
            return newSetup;
        }
    }
}
