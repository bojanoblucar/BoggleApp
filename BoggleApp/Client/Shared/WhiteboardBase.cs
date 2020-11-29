using System;
using System.Collections.Generic;
using System.Linq;
using BoggleApp.Client.Services;
using BoggleApp.Shared.Enums;
using BoggleApp.Shared.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BoggleApp.Client.Shared
{
    public class WhiteboardBase : ComponentBase
    {
        [Inject] public IGameScoreClientService GameScoreClientService { get; set; }

        protected List<Word> Words = new List<Word>();

        protected string Score { get; private set; }

        [Parameter] public Action<int, int> OnScoreChanged { get; set; }

        private int _score;

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected override void OnInitialized()
        {
            InitScore();
        }

        public void AddWord(string text)
        {
            var inputs = text.Split();
            foreach (var input in inputs)
            {
                if (!string.IsNullOrEmpty(input) && input.Length > 2 && !Words.Any(w => w.Value.Equals(input.ToLower())))
                    Words.Add(new Word(input.ToLower()));
            }          
            StateHasChanged();
        }

        public int GetPoints()
        {
            return Words.Select(w => w.Points).Sum();
        }


        public void Clear()
        {
            InitScore();
            Words.Clear();
            StateHasChanged();
        }

        public void OnChipClicked(Word w)
        {
            GameScoreClientService.ValidateWord(w);
            var newScore = GetPoints();
            var diff = newScore - _score;
            _score = newScore;
            Score = _score.ToString();

            OnScoreChanged?.Invoke(newScore, diff);           

            StateHasChanged();
        }



        private void InitScore()
        {
            _score = 0;
            Score = "--";
        }
    }
}
