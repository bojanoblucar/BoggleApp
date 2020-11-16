using System;
using BoggleApp.Shared.Shared;

namespace BoggleApp.Client.Services
{
    public class GameScoreService : IGameScoreClientService
    {
        private readonly GameRules gameRules;

        public GameScoreService(GameRules gameRules)
        {
            this.gameRules = gameRules;
        }

        public void ValidateWord(Word word)
        {
            int pnt = gameRules.GetPoints(word.Value);
            word.Points = pnt;
        }
    }
}
