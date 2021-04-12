using BoggleApp.Shared.Hub;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.HubHelpers
{
    public interface IHubRequest<T>
    {
        public string RequestName { get;}

        public T RequestMessage { get;}
    }
}
