using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class EditProjectsDialogViewModel : INotifyPropertyChanged
    {
        private List<Model.Project> _projects;
        private object _selectedItem;
        private bool _canSelect;

        public event PropertyChangedEventHandler PropertyChanged;

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
            get { return _projects; }
        }

        public Model.Project SelectedProject
        {
            get { return (Model.Project)_selectedItem; }
        }

        public async Task DeleteProject()
        {
            var team = (Model.Project)SelectedItem;
            await Controller.Instance.UnitOfWork.ProjectsAsync.RemoveAsync(team);

            _projects.Remove(team);
            OnPropertyChanged("Projects");
        }

        public async Task LoadProjects()
        {
            _projects = new List<Model.Project>(await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync());
            OnPropertyChanged("Projects");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
