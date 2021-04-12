using BoggleApp.Shared.Helpers;
using BoggleApp.Shared.Hub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.HubHelpers
{
    public class HubCommunicator
    {
  
        private readonly NavigationManager navigationManager;

        public HubCommunicator(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        public HubConnection HubConnection { get; private set; }

        public async Task SetupAuthenticated(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("No token");

            HubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/chathub"), options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .Build();

            await HubConnection.StartAsync();
        }

        public async Task SendAsync<TReq>(IHubRequest<TReq> request)
        {
            await HubConnection.SendAsync(request.RequestName, request.RequestMessage);
        }

        public void On<T>(string responseName, Action<T> action)
        {
            HubConnection.On(responseName, action);
        }
    }
}
