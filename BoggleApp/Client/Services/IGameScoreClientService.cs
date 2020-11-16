using System;
using BoggleApp.Shared.Shared;

namespace BoggleApp.Client.Services
{
    public interface IGameScoreClientService
    {
        void ValidateWord(Word word);
    }
}
