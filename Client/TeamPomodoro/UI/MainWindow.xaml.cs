using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

namespace TeamPomodoro
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		void OnLoaded(object sender, RoutedEventArgs e)
		{
			Controller.Create(this);
			Controller.Instance.ShowSignIn();
		}
	}
}
