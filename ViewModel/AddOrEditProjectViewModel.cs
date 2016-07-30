using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AddOrEditProjectViewModel : INotifyPropertyChanged
    {
        private string _projectName;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
                OnPropertyChanged();
            }
        }

        public async Task UpdateProject(string projectName)
        {
            var p = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAsync(Controller.Instance.CurrentProject.ProjectId);
            p.Name = projectName;
            Controller.Instance.CurrentProject = p;
            await Controller.Instance.UnitOfWork.SaveChangesAsync();
        }

        public async Task AddProject(string projectName)
        {
            var project = new Model.Project
            {
                ProjectId = Guid.NewGuid(),
                Name = projectName
            };

            await Controller.Instance.UnitOfWork.ProjectsAsync.AddAsync(project);
        }

        public void InitializeValues()
        {
            ProjectName = Controller.Instance.CurrentProject.Name;
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