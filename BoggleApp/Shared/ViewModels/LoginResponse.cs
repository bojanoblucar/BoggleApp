using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Shared.ViewModels
{
    public class LoginResponse
    {
        public UserViewModel User { get; set; }

        public string Token { get; set; }
    }
}
