using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ViewModel;
using ViewModel.Globalization;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class PomodoroDetails : Window
    {
        public PomodoroDetails()
        {
            InitializeComponent();
        }

        public async Task Initialize()
        {
            var viewModel = (PomodoroDetailsViewModel)DataContext;
            await viewModel.Initialize();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }
    }
}
