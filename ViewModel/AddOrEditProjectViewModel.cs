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
    public class AddOrEditProjectViewModel : INotifyPropertyChanged
    {
        private string _projectName;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private async void Save()
        {
            Controller.Instance.CurrentProject.Name = ProjectName;
            await Controller.Instance.UnitOfWork.SaveChangesAsync();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
    }
}