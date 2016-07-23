using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace TeamPomodoro.UI
{
    public partial class AddOrRename : Window
    {
        public AddOrRename()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}