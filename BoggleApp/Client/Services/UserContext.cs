using BlazorBrowserStorage;
using BoggleApp.Client.Extensions;
using BoggleApp.Shared.Helpers;
using BoggleApp.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoggleApp.Client.Services
{
    public class UserContext : IUserContext
    {
        private readonly ISessionStorage storage;

        public UserContext(ISessionStorage stotage)
        {
            this.storage = stotage;
        }

        public Task<string> GetToken()
        {
            return storage.GetItemModified<string>("token");
        }

        public Task<UserViewModel> GetUser()
        {
            return storage.GetItemModified<UserViewModel>("username");
        }

        public void SetToken(string token)
        {
            storage.SetItem("token", token);
        }

        public void SetUser(UserViewModel user)
        {
            storage.SetItem("username", user);
        }
    }
}
