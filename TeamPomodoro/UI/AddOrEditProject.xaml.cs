using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;
using ViewModel.Globalization;

namespace TeamPomodoro.UI
{
    public partial class AddOrEditProject : Window
    {
        public AddOrEditProject()
        {
            InitializeComponent();
            DataContext = new AddOrEditProjectViewModel();
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
                var viewModel = (AddOrEditProjectViewModel)FindResource("AddOrEditProjectViewModel");
                if (IsEdit)
                {
                    await viewModel.UpdateProject(text.Text);
                }
                else
                {
                    await viewModel.AddProject(text.Text);
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
            var edit = new AddOrEditProject
            {
                Owner = owner,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                IsEdit = isEdit
            };

            var viewModel = (AddOrEditProjectViewModel)edit.FindResource("AddOrEditProjectViewModel");
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