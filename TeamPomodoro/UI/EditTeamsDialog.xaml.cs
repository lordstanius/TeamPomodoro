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
    public partial class EditTeamsDialog : Window
    {
        public EditTeamsDialog()
        {
            InitializeComponent();
        }

        public static async Task ShowEditDialog(Window owner)
        {
            try
            {
                var edit = new EditTeamsDialog
                {
                    Owner = owner,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var viewModel = (EditTeamsDialogViewModel)edit.FindResource("EditTeamsDialogViewModel");
                await viewModel.LoadTeams();

                edit.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditTeamsDialog.ShowEditDialog()");
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
                var viewModel = (EditTeamsDialogViewModel)FindResource("EditTeamsDialogViewModel");
                await viewModel.DeleteTeam();
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
                AddOrEditTeam.ShowEditDialog(this, true);
                var viewModel = (EditTeamsDialogViewModel)FindResource("EditTeamsDialogViewModel");
                await viewModel.LoadTeams();
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
                AddOrEditTeam.ShowEditDialog(this, false);
                var viewModel = (EditTeamsDialogViewModel)FindResource("EditTeamsDialogViewModel");
                await viewModel.LoadTeams();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddClick()");
            }

            list.Items.Refresh();
        }
    }
}
