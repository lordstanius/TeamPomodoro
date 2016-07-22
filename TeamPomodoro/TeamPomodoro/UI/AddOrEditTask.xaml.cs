using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

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

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		private void OnSaveClick(object sender, RoutedEventArgs e)
		{
			DialogResult = Controller.Instance.ValidateTask(this);
		}
	}
}
