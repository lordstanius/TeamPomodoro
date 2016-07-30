using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        private int _pomodoroDuration = 24;
        private ICollection<Model.Team> _teams;
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
                OnPropertyChanged();
            }
        }

        public string Password { get; set; }

        public ICollection<Model.Team> Teams
        {
            get { return _teams; }
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
                OnPropertyChanged();
            }
        }

        public bool ShowWarning
        {
            get
            {
                return _showWarning;
            }
            set
            {
                _showWarning = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
            }
        }

        public async Task Initialize(string userName)
        {
            _teams = new List<Model.Team>(await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync());
            OnPropertyChanged("Teams");

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

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}