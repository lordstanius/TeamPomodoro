using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModel
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        //public static readonly DependencyProperty PomodoroDurationProperty = DependencyProperty.Register(
        //    "PomodoroDuration",
        //    typeof(int),
        //    typeof(UserDetailsViewModel),
        //    new PropertyMetadata(24));

        private int _pomodoroDuration = 24;
        private List<Model.Team> _teams;
        private bool _showWarning = true;
        private bool _isUserNameEnabled = true;
        private string _userName;
        private Model.Team _selectedTeam;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
                }
            }
        }

        public string Password { get; set; }

        public List<Model.Team> Teams
        {
            get
            {
                return _teams;
            }
            set
            {
                _teams = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Teams"));
                }
            }
        }

        public int PomodoroDuration
        {
            get
            {
                return _pomodoroDuration;
            }
            set
            {
                _pomodoroDuration = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PomodoroDuration"));
                }
            }
        }

        public bool ShowWarning
        {
            get
            {
                return _showWarning;
            }
            set { _showWarning = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ShowWarning"));
                }
            }
        }

        public Model.Team SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                _selectedTeam = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedTeam"));
                }
            }
        }

        public bool IsUserNameEnabled
        {
            get
            {
                return _isUserNameEnabled;
            }
            set
            {
                _isUserNameEnabled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsUserNameEnabled"));
                }
            }
        }

        public async Task Initialize(string userName)
        {
            Teams = new List<Model.Team>(await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync());

            if (Controller.Instance.User != null)
            {
                if (Controller.Instance.User.TeamId != null)
                {
                    SelectedTeam = Controller.Instance.UnitOfWork.Teams.GetById((Guid)Controller.Instance.User.TeamId);
                }

                IsUserNameEnabled = false;
                ShowWarning = Controller.Instance.User.ShowWarningAfterPomodoroExpires;
                PomodoroDuration = Controller.Instance.User.PomodoroDurationInMin;
                Password = Controller.Instance.UserCredential.Password;
                UserName = userName ?? Controller.Instance.User.UserName;
            }
            else
            {
                IsUserNameEnabled = true;
                UserName = userName;
            }
        }

        public bool ValidateUserName()
        {
            return Controller.Instance.ValidateUserName(UserName);
        }

        public bool ValidatePassword()
        {
            return Controller.Instance.ValidatePassword(Password);
        }

        public async void UpdateUser()
        {
            Guid? teamId = null;
            if (SelectedTeam != null)
            {
                teamId = SelectedTeam.TeamId;
            }

            // TODO: Take care of password handling
            await Controller.Instance.UpdateUser(
                UserName,
                Password,
                PomodoroDuration,
                ShowWarning,
                teamId);

            await Controller.Instance.UnitOfWork.SaveChangesAsync();
        }
    }
}