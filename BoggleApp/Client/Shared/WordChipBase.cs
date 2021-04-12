using System;
using BoggleApp.Game.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BoggleApp.Client.Shared
{
    public class WordChipBase : ComponentBase
    {
        [Parameter] public BoggleApp.Shared.Shared.Word Text { get; set; }

        protected override void OnInitialized()
        {
            background = "#e4e4e4";
            StateHasChanged();
        }

        protected string background = "#e4e4e4";

        public void OnClick(MouseEventArgs e)
        {
            if (e.Button == 0)
                AcceptAnswer();
            else if (e.Button == 2)
                RejectAnswer();
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == 2)
                RejectAnswer();
        }

        public void OnDblClick(MouseEventArgs e)
        {
            RejectAnswer();
        }


        private void AcceptAnswer()
        {
            background = "yellowgreen";
            Text.Status = WordStatus.Correct;
            OnChipClick?.Invoke(Text);
            StateHasChanged();
        }

        protected void RejectAnswer()
        {
            background = "coral";
            Text.Status = WordStatus.False;
            OnChipClick?.Invoke(Text);
            StateHasChanged();
        }
        
        [Parameter] public Action<BoggleApp.Shared.Shared.Word> OnChipClick { get; set; }
    }
}
