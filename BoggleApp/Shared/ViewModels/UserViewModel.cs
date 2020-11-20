using System;
using BoggleApp.Shared.Enums;

namespace BoggleApp.Shared.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }

        public string Username { get; set; }

        public string Id { get; set; }

        public ConnectionStatus ConnectionStatus { get; set; }

        public int Score { get; set; }
    }
}
