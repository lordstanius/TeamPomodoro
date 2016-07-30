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
    public partial class EditProjectsDialog : Window
    {
        public EditProjectsDialog()
        {
            InitializeComponent();
        }

        public static async Task ShowEditDialog(Window owner)
        {
            try
            {
                var edit = new EditProjectsDialog
                {
                    Owner = owner,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var viewModel = (EditProjectsDialogViewModel)edit.FindResource("EditProjectsDialogViewModel");
                await viewModel.LoadProjects();

                edit.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditProjectsDialog.ShowEditDialog()");
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
                var viewModel = (EditProjectsDialogViewModel)FindResource("EditProjectsDialogViewModel");
                await viewModel.DeleteProject();
                list.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnDeleteTask()");
            }
        }

        private async void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AddOrEditProject.ShowEditDialog(this, true);
                var viewModel = (EditProjectsDialogViewModel)FindResource("EditProjectsDialogViewModel");
                await viewModel.LoadProjects();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddClick()");
            }

            list.Items.Refresh();
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AddOrEditProject.ShowEditDialog(this, false);
                var viewModel = (EditProjectsDialogViewModel)FindResource("EditProjectsDialogViewModel");
                await viewModel.LoadProjects();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddClick()");
            }

            list.Items.Refresh();
        }
    }
}
