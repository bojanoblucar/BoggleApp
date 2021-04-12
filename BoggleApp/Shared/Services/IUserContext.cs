using BoggleApp.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoggleApp.Shared.Helpers
{
    public interface IUserContext
    {
        Task<UserViewModel> GetUser();

        void SetUser(UserViewModel user);

        Task<string> GetToken();

        void SetToken(string token);
    }
}
