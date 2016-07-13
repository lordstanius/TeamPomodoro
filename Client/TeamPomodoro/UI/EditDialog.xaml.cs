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
	public partial class EditDialog : Window
	{
		public EditDialog(bool isTeam)
		{
			InitializeComponent();

			if (isTeam)
			{
				Title = Strings.TxtAddTeam;
				lTitle.Text = Strings.TxtTeamName;
			}
			else
			{
				Title = Strings.TxtAddProject;
				lTitle.Text = Strings.TxtProjectName;
			}
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}
	}
}
