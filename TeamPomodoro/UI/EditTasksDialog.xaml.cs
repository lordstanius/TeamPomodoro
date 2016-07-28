using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class EditTasksDialog : Window
    {
        public EditTasksDialog()
        {
            InitializeComponent();
        }

        public static async Task ShowEditDialog(Window owner)
        {
            try
            {
                var edit = new EditTasksDialog
                {
                    Owner = owner,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var viewModel = (EditTasksDialogViewModel)edit.FindResource("EditTasksDialogViewModel");
                await viewModel.GetTasks();

                edit.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.ShowEditDialog()");
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            btnEdit.IsEnabled = false;
            try
            {
                var viewModel = (EditTasksDialogViewModel)FindResource("EditTasksDialogViewModel");
                await viewModel.DeleteTask();
                list.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnDeleteTask()");
            }
        }

        private async void OnEditClick(object sender, RoutedEventArgs e)
        {
            await AddOrEditTask.ShowEditDialog(this, true);
            try
            {
                var viewModel = (EditTasksDialogViewModel)FindResource("EditTasksDialogViewModel");
                await viewModel.GetTasks();
                list.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddClick()");
            }
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            await AddOrEditTask.ShowEditDialog(this, false);
            try
            {
                var viewModel = (EditTasksDialogViewModel)FindResource("EditTasksDialogViewModel");
                await viewModel.GetTasks();
                list.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddClick()");
            }
        }
    }
}
