using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BoggleApp.Client.Shared
{
    public class WordBase : ComponentBase
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


        private void AcceptAnswer()
        {
            background = "yellowgreen";
            OnChipClick?.Invoke(Text);
            StateHasChanged();
        }

        protected void RejectAnswer()
        {
            background = "coral";
            Text.Status = BoggleApp.Shared.Enums.WordStatus.False;
            StateHasChanged();
        }
        
        [Parameter] public Action<BoggleApp.Shared.Shared.Word> OnChipClick { get; set; }
    }
}
