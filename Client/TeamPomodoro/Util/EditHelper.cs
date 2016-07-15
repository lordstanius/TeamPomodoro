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
		public enum EditType { Project, Team };

		EditType _type;
		EditDialog _dlg;
		AddOrRename _edit;

		public EditHelper(EditType type)
		{
			_type = type;
		}

		void CreateDialog()
		{
			_dlg = new EditDialog
			{
				Owner = Controller.Instance.MainWindow,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};

			_dlg.btnAdd.Click += OnAdd;
			_dlg.btnDelete.Click += OnDelete;
			_dlg.btnEdit.Click += OnEdit;

			switch (_type)
			{
				case EditType.Project:
					_dlg.Title = Strings.TxtProjects;
					break;

				case EditType.Team:
					_dlg.Title = Strings.TxtTeams;
					break;
			}
		}

		void CreateEdit()
		{
			_edit = new AddOrRename
			{
				Owner = Controller.Instance.MainWindow,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};

			switch (_type)
			{
				case EditType.Project:
					_edit.Title = Strings.TxtEditProject;
					_edit.name.Text = Strings.TxtProjectName;
					break;

				case EditType.Team:
					_edit.Title = Strings.TxtEditTeam;
					_edit.name.Text = Strings.TxtTeamName;
					break;
			}
		}

		async Task GetItems()
		{
			_dlg.list.Items.Clear();

			switch (_type)
			{
				case EditType.Project:
					var projects = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync();
					foreach (var item in projects)
						_dlg.list.Items.Add(item);
					break;
				case EditType.Team:
					var teams = await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync();
					foreach (var items in teams)
						_dlg.list.Items.Add(items);
					break;
			}

			_dlg.list.Items.Refresh();
		}

		internal async Task ShowEditDialog()
		{
			Controller.Instance.MainWindow.Cursor = Cursors.Wait;

			try
			{
				CreateDialog();

				await GetItems();

				_dlg.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.Message);
			}

			Controller.Instance.MainWindow.Cursor = Cursors.Arrow;
		}

		async void OnEdit(object sender, RoutedEventArgs e)
		{
			CreateEdit();
			_edit.text.Text = ((Model.Team)_dlg.list.SelectedItem).Name;
			_edit.text.Focus();
			_edit.text.SelectAll();

			if (_edit.ShowDialog() == true)
			{
				try
				{
					switch (_type)
					{
						case EditType.Team:
							var team = (Model.Team)_dlg.list.SelectedItem;
							Model.Team t = await Controller.Instance.UnitOfWork.TeamsAsync.GetAsync(team.Id());
							t.Name = _edit.text.Text;
							team.Name = t.Name;
							break;

						case EditType.Project:
							var project = (Model.Project)_dlg.list.SelectedItem;
							Model.Project p = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAsync(project.Id());
							p.Name = _edit.text.Text;
							project.Name = p.Name;
							break;
					}

					_dlg.list.Items.Refresh();
					await Controller.Instance.UnitOfWork.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					MessageDialog.ShowError(ex.Message);
				}
			}
		}

		async void OnDelete(object sender, RoutedEventArgs e)
		{
			_dlg.btnEdit.IsEnabled = false;
			try
			{
				switch (_type)
				{
					case EditType.Team:
						var team = (Model.Team)_dlg.list.SelectedItem;
						await Controller.Instance.UnitOfWork.TeamsAsync.RemoveAsync(team);
						break;

					case EditType.Project:
						var project = (Model.Project)_dlg.list.SelectedItem;
						await Controller.Instance.UnitOfWork.ProjectsAsync.RemoveAsync(project);
						break;
				}

				_dlg.list.Items.Remove(_dlg.list.SelectedItem);
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.Message);
			}
		}

		async void OnAdd(object sender, RoutedEventArgs e)
		{
			CreateEdit();
			_edit.text.Focus();
			if (_edit.ShowDialog() == true)
			{
				try
				{
					switch (_type)
					{
						case EditType.Team:
							var team = new Model.Team
							{
								TeamId = Guid.NewGuid(),
								Name = _edit.text.Text
							};

							_dlg.list.Items.Add(team);
							await Controller.Instance.UnitOfWork.TeamsAsync.AddAsync(team);
							break;

						case EditType.Project:
							var project = new Model.Project
							{
								ProjectId = Guid.NewGuid(),
								Name = _edit.text.Text
							};

							_dlg.list.Items.Add(project);
							await Controller.Instance.UnitOfWork.ProjectsAsync.AddAsync(project);
							break;
					}
				}
				catch (Exception ex)
				{
					MessageDialog.ShowError(ex.Message);
				}
			}
		}
	}
}

