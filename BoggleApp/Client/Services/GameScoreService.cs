using System;
using BoggleApp.Game.Enums;
using BoggleApp.Game.Setup;
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
            if (word.Status == WordStatus.Correct)
            {
                int pnt = gameRules.GetPoints(word.Value);
                word.Points = pnt;
            }
        }
    }
}
