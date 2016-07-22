using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using TeamPomodoro.UI;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

namespace TeamPomodoro.Util
{
    class EditHelper
    {
        public enum EditType { Project, Team, Task };

        EditType _type;
        EditDialog _edit;
        AddOrRename _addRename;
        AddOrEditTask _editTask;

        public EditHelper(EditType type)
        {
            _type = type;
        }

        void CreateEditDialog()
        {
            _edit = new EditDialog
            {
                Owner = Controller.Instance.Main,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            switch (_type)
            {
                case EditType.Project:
                    _edit.Title = Strings.TxtProjects;
                    goto default;

                case EditType.Team:
                    _edit.Title = Strings.TxtTeams;
                    goto default;

                case EditType.Task:
                    _edit.btnAdd.Click += OnAddTask;
                    _edit.btnDelete.Click += OnDeleteTask;
                    _edit.btnEdit.Click += OnEditTask;
                    _edit.Title = Strings.TxtTasks;
                    break;

                default:
                    _edit.btnAdd.Click += OnAdd;
                    _edit.btnDelete.Click += OnDelete;
                    _edit.btnEdit.Click += OnEdit;
                    break;
            }
        }

        void CreateAddOrRenameDialog()
        {
            _addRename = new AddOrRename
            {
                Owner = Controller.Instance.Main,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            switch (_type)
            {
                case EditType.Project:
                    _addRename.Title = Strings.TxtEditProject;
                    _addRename.name.Text = Strings.TxtProjectName;
                    break;

                case EditType.Team:
                    _addRename.Title = Strings.TxtEditTeam;
                    _addRename.name.Text = Strings.TxtTeamName;
                    break;
            }
        }

        void CreateEditTaskDialog()
        {
            _editTask = new AddOrEditTask
            {
                Owner = Controller.Instance.Main,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
        }

        async Task GetItems()
        {
            _edit.list.Items.Clear();

            switch (_type)
            {
                case EditType.Project:
                    var projects = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync();
                    foreach (var proj in projects)
                        _edit.list.Items.Add(proj);
                    break;

                case EditType.Team:
                    var teams = await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync();
                    foreach (var team in teams)
                        _edit.list.Items.Add(team);
                    break;

                case EditType.Task:
                    var user = await Controller.Instance.UnitOfWork.UsersAsync.GetAsync(Controller.Instance.User.UserId);
                    foreach (var task in user.Tasks)
                        _edit.list.Items.Add(task);

                    _edit.list.SelectedItem = Controller.Instance.Main.tasks.SelectedItem;
                    break;
            }

            _edit.list.Items.Refresh();
        }

        internal async Task ShowEditDialog()
        {
            Controller.Instance.Main.Cursor = Cursors.Wait;

            try
            {
                CreateEditDialog();

                await GetItems();

                _edit.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.ShowEditDialog()");
            }

            Controller.Instance.Main.Cursor = Cursors.Arrow;
        }

        async void OnEdit(object sender, RoutedEventArgs e)
        {
            CreateAddOrRenameDialog();

            _addRename.text.Text = _edit.list.SelectedItem.ToString();
            _addRename.text.Focus();
            _addRename.text.SelectAll();

            if (_addRename.ShowDialog() == true)
            {
                try
                {
                    switch (_type)
                    {
                        case EditType.Team:
                            var team = (Model.Team)_edit.list.SelectedItem;
                            Model.Team t = await Controller.Instance.UnitOfWork.TeamsAsync.GetAsync(team.GetId());
                            t.Name = _addRename.text.Text;
                            _edit.list.Items[_edit.list.SelectedIndex] = t;
                            break;

                        case EditType.Project:
                            var project = (Model.Project)_edit.list.SelectedItem;
                            Model.Project p = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAsync(project.GetId());
                            p.Name = _addRename.text.Text;
                            _edit.list.Items[_edit.list.SelectedIndex] = p;
                            break;
                    }

                    _edit.list.Items.Refresh();
                    await Controller.Instance.UnitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowError(ex, "EditHelper.OnEdit()");
                }
            }
        }

        async void OnDelete(object sender, RoutedEventArgs e)
        {
            _edit.btnEdit.IsEnabled = false;
            try
            {
                switch (_type)
                {
                    case EditType.Team:
                        var team = (Model.Team)_edit.list.SelectedItem;
                        await Controller.Instance.UnitOfWork.TeamsAsync.RemoveAsync(team);
                        break;

                    case EditType.Project:
                        var project = (Model.Project)_edit.list.SelectedItem;
                        await Controller.Instance.UnitOfWork.ProjectsAsync.RemoveAsync(project);
                        break;
                }

                _edit.list.Items.Remove(_edit.list.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnDelete()");
            }
        }

        async void OnAdd(object sender, RoutedEventArgs e)
        {
            CreateAddOrRenameDialog();
            _addRename.text.Focus();
            if (_addRename.ShowDialog() == true)
            {
                try
                {
                    switch (_type)
                    {
                        case EditType.Team:
                            var team = new Model.Team
                            {
                                TeamId = Guid.NewGuid(),
                                Name = _addRename.text.Text
                            };

                            _edit.list.Items.Add(team);
                            await Controller.Instance.UnitOfWork.TeamsAsync.AddAsync(team);
                            break;

                        case EditType.Project:
                            var project = new Model.Project
                            {
                                ProjectId = Guid.NewGuid(),
                                Name = _addRename.text.Text
                            };

                            _edit.list.Items.Add(project);
                            await Controller.Instance.UnitOfWork.ProjectsAsync.AddAsync(project);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowError(ex, "EditHelper.OnAdd()");
                }
            }
        }

        private async void OnEditTask(object sender, RoutedEventArgs e)
        {
            CreateEditTaskDialog();

            Model.Task task = null;
            try
            {
                _edit.Cursor = Cursors.Wait;

                task = (Model.Task)_edit.list.SelectedItem;
                task = await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(task.TaskId);
            }
            finally
            {
                _edit.Cursor = Cursors.Arrow;
            }
            _editTask.Title = Strings.TxtEditTask;
            _editTask.IsOfEditType = true;
            _editTask.lChoose.Content = (await Controller.Instance.UnitOfWork.ProjectsAsync.GetAsync(task.ProjectId)).ToString();
            _editTask.lChoose.Visibility = Visibility.Visible;
            _editTask.projects.IsEnabled = false;
            _editTask.numPomodoros.Minimum = task.Pomodoroes.Count;

            _editTask.text.Text = task.Name;
            _editTask.text.Focus();
            _editTask.text.SelectAll();

            _editTask.numPomodoros.Number = task.PomodoroCount;

            if (_editTask.ShowDialog() == true)
            {
                try
                {
                    Model.Task t = await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(task.TaskId);
                    t.Name = _editTask.text.Text;
                    t.PomodoroCount = _editTask.numPomodoros.Number;

                    _edit.list.Items[_edit.list.SelectedIndex] = t;
                    _edit.list.Items.Refresh();
                    await Controller.Instance.UnitOfWork.SaveChangesAsync();

                    int index = Controller.Instance.Main.tasks.SelectedIndex;
                    Controller.Instance.Main.tasks.Items[index] = t;
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowError(ex, "EditHelper.OnEditTask()");
                }
            }
        }

        private async void OnDeleteTask(object sender, RoutedEventArgs e)
        {
            _edit.btnEdit.IsEnabled = false;
            try
            {
                var task = (Model.Task)_edit.list.SelectedItem;
                await Controller.Instance.UnitOfWork.TasksAsync.RemoveAsync(task);

                _edit.list.Items.Remove(_edit.list.SelectedItem);
                Controller.Instance.Main.tasks.Items.Remove(task);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnDeleteTask()");
            }
        }

        private async void OnAddTask(object sender, RoutedEventArgs e)
        {
            CreateEditTaskDialog();

            var projects = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync();
            _editTask.IsOfEditType = false;
            _editTask.Title = Strings.TxtAdd;
            _editTask.projects.ItemsSource = projects;
            _editTask.projects.IsEnabled = _editTask.projects.Items.Count > 0;
            _editTask.projects.DropDownClosed += (o, a) => _editTask.text.Focus();
            _editTask.text.Focus();

            if (_editTask.ShowDialog() == true)
            {
                try
                {
                    var task = new Model.Task
                    {
                        TaskId = Guid.NewGuid(),
                        UserId = Controller.Instance.User.UserId,
                        TeamId = Controller.Instance.User.TeamId,
                        Name = _editTask.text.Text,
                        ProjectId = ((Model.Project)_editTask.projects.SelectedItem).ProjectId,
                        PomodoroCount = _editTask.numPomodoros.Number
                    };

                    _edit.list.Items.Add(task);
                    Controller.Instance.Main.tasks.Items.Add(task);
                    await Controller.Instance.UnitOfWork.TasksAsync.AddAsync(task);
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowError(ex, "EditHelper.OnAddTask()");
                }
            }
        }
    }
}

