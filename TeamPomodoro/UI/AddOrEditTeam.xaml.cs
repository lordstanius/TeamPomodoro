using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;
using ViewModel.Globalization;

namespace TeamPomodoro.UI
{
    public partial class AddOrEditTeam : Window
    {
        public AddOrEditTeam()
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
            try
            {
                var viewModel = (AddOrEditTeamViewModel)FindResource("AddOrEditTeamViewModel");
                if (IsEdit)
                {
                    await viewModel.UpdateTeam(text.Text);
                }
                else
                {
                    await viewModel.AddTeam(text.Text);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "EditHelper.OnAddTask()");
            }

            DialogResult = true;
        }

        public static void ShowEditDialog(Window owner, bool isEdit)
        {
            var edit = new AddOrEditTeam
            {
                Owner = owner,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                IsEdit = isEdit,
            };
            
            var viewModel = (AddOrEditTeamViewModel)edit.FindResource("AddOrEditTeamViewModel");
            edit.text.Focus();

            if (isEdit)
            {
                viewModel.InitializeValues();
                edit.text.SelectAll();
            }

            edit.ShowDialog();
        }
    }
}