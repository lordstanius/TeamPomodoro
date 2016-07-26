using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;
using ViewModel.Globalization;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class AddOrEditTask : Window
    {
        public AddOrEditTask()
        {
            InitializeComponent();
        }

        public bool IsEdit { get; set; }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (task.Text.Length == 0)
            {
                MessageDialog.Show(this, Strings.MsgTaskNameIsEmpty);
                return;
            }

            if (projects.SelectedItem == null)
            {
                MessageDialog.Show(this, Strings.MsgAddProjects);
                return;
            }

            try
            {
                var viewModel = (AddOrEditTaskViewModel)FindResource("AddOrEditTaskViewModel");
                if (IsEdit)
                {
                    await viewModel.UpdateTask(task.Text);
                }
                else
                {
                    await viewModel.AddTask(task.Text);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddTask()");
            }

            DialogResult = true;
        }

        public static async Task ShowEditDialog(Window owner, bool isEdit)
        {
            var edit = new AddOrEditTask
            {
                Owner = owner,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                IsEdit = isEdit
            };

            var viewModel = (AddOrEditTaskViewModel)edit.FindResource("AddOrEditTaskViewModel");
            await viewModel.GetProjects();

            edit.projects.DropDownClosed += (o, a) => edit.task.Focus();
            edit.task.Focus();

            if (isEdit)
            {
                viewModel.InitializeValues();
                edit.task.SelectAll();

                edit.numPomodoros.Minimum = viewModel.GetMinPomodoroCount();
            }

            edit.ShowDialog();
        }
    }
}
