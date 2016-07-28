﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class EditTeamsDialogViewModel : INotifyPropertyChanged
    {
        private List<Model.Team> _teams;
        private object _selectedItem;
        private bool _canSelect;

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                CanSelect = _selectedItem != null;
                Controller.Instance.CurrentTeam = SelectedTeam;
                OnPropertyChanged();
            }
        }

        public bool CanSelect
        {
            get
            {
                return _canSelect;
            }
            set
            {
                _canSelect = value;
                OnPropertyChanged();
            }
        }

        public List<Model.Team> Teams
        {
            get
            {
                return _teams;
            }
            set
            {
                _teams = value;
                OnPropertyChanged();
            }
        }

        public Model.Team SelectedTeam { get { return (Model.Team)_selectedItem; } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task DeleteTeam()
        {
            var team = (Model.Team)SelectedItem;
            await Controller.Instance.UnitOfWork.TeamsAsync.RemoveAsync(team);

            Teams.Remove(team);
        }

        public async Task GetTeams()
        {
            Teams = new List<Model.Team>(await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync());
        }
    }
}