using System.Security;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModel
{
    public class SignInViewModel : DependencyObject
    {
        public string UserName { get; set; }

        public SecureString Password { get; set; }

        public Task<bool?> GetUser()
        {
            return Controller.Instance.LoadUser(UserName, Password);
        }

        public bool ValidateUser(string userName)
        {
            UserName = userName;
            return Controller.Instance.ValidateUserName(UserName);
        }
    }
}