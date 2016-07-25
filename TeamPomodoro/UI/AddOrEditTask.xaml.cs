using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

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

        public bool IsOfEditType { get; internal set; }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
