using System;
using System.Collections.Generic;
using System.Linq;
using BoggleApp.Client.Services;
using BoggleApp.Shared.Shared;
using Microsoft.AspNetCore.Components;

namespace BoggleApp.Client.Shared
{
    public class WhiteboardBase : ComponentBase
    {
        [Inject] public IGameScoreClientService GameScoreClientService { get; set; }

        protected List<BoggleApp.Shared.Shared.Word> Words = new List<BoggleApp.Shared.Shared.Word>();

        protected string Score { get; private set; }

        protected override void OnInitialized()
        {
            InitScore();
        }

        public void AddWord(string text)
        {
            var inputs = text.Split();
            foreach (var input in inputs)
            {
                if (!string.IsNullOrEmpty(input) && !Words.Any(w => w.Value.Equals(input.ToLower())))
                    Words.Add(new BoggleApp.Shared.Shared.Word(input.ToLower()));
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

        public void OnChipClicked(BoggleApp.Shared.Shared.Word w)
        {
            GameScoreClientService.ValidateWord(w);
            Score = GetPoints().ToString();
            StateHasChanged();
        }

        private void InitScore()
        {
            Score = "--";
        }
    }
}
