using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace TeamPomodoro.UI
{
    public partial class AddOrEditTeam : Window
    {
        public AddOrEditTeam()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}