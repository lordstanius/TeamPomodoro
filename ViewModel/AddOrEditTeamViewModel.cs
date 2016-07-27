using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class AddOrEditTeamViewModel : INotifyPropertyChanged
    {
        private string _teamName;

        public string TeamName
        {
            get
            {
                return _teamName;
            }
            set
            {
                _teamName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void Save()
        {
            Controller.Instance.CurrentTeam.Name = TeamName;
            await Controller.Instance.UnitOfWork.SaveChangesAsync();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task UpdateTeam(string teamName)
        {
            var t = await Controller.Instance.UnitOfWork.TeamsAsync.GetAsync(Controller.Instance.CurrentTeam.TeamId);
            t.Name = teamName;
            Controller.Instance.CurrentTeam = t;
            await Controller.Instance.UnitOfWork.SaveChangesAsync();
        }

        public async Task AddTeam(string teamName)
        {
            var team = new Model.Team
            {
                TeamId = Guid.NewGuid(),
                Name = teamName
            };

            await Controller.Instance.UnitOfWork.TeamsAsync.AddAsync(team);
        }

        public void InitializeValues()
        {
            TeamName = Controller.Instance.CurrentTeam.Name;
        }
    }
}