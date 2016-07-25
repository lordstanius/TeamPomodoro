using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ViewModel
{
    public class SignInViewModel : DependencyObject
    {
        public string UserName { get; set; }

        public SecureString Password { get; set; }

        public Task<bool?> GetUser()
        {
            return Controller.Instance.GetUser(UserName, Password);
        }

        public bool ValidateUser(string userName)
        {
            UserName = userName;
            return Controller.Instance.ValidateUserName(UserName);
        }
    }
}