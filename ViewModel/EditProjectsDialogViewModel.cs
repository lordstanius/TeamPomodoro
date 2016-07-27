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
    public class EditProjectsDialogViewModel : INotifyPropertyChanged
    {
        private List<Model.Project> _projects;
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
                Controller.Instance.CurrentProject = SelectedProject;
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

        public List<Model.Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
                OnPropertyChanged();
            }
        }

        public Model.Project SelectedProject { get { return (Model.Project)_selectedItem; } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task DeleteProject()
        {
            var team = (Model.Project)SelectedItem;
            await Controller.Instance.UnitOfWork.ProjectsAsync.RemoveAsync(team);

            Projects.Remove(team);
        }

        public async Task GetProjects()
        {
            Projects = new List<Model.Project>(await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync());
        }
    }
}
