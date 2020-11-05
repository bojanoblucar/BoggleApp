using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BoggleApp.Client.Shared
{
    public class BoggleBoardBase : ComponentBase
    {
        [Parameter] public string RoomId { get; set; }

        [CascadingParameter] public HubConnection HubConnection { get; set; }

        protected string[] letters;

        protected override void OnInitialized()
        {
            HubConnection.On<string[]>("ReceiveShuffled", (message) =>
            {
                letters = message;
                StateHasChanged();
            });
        }

        public Task Shuffle() => HubConnection.SendAsync("Shuffle", RoomId);

        protected string[] GetBoardRow(int rowNumber)
        {
            string[] result = new string[4];
            Array.Copy(letters, rowNumber * 4, result, 0, 4);
            return result;
        }
    }
}
