using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace TeamPomodoro.UI
{
    public partial class AddOrEditProject : Window
    {
        public AddOrEditProject()
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