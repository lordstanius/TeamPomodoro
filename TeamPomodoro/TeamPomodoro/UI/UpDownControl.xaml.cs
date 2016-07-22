using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamPomodoro.UI
{
	/// <summary>
	/// Interaction logic for UpDownControl.xaml
	/// </summary>
	public partial class UpDownControl : UserControl
	{
		public int Value
		{
			get { return int.Parse(txtNumVal.Text); }
			set
			{
				if (value > Maximum || value < Minimum)
					return;
				txtNumVal.Text = value.ToString();
			}
		}

		public int Minimum { get; set; }
		public int Maximum { get; set; } 

		public UpDownControl()
		{
			InitializeComponent();

			Minimum = 0;
			Maximum = 100;
		}

		void OnDownClick(object sender, RoutedEventArgs e)
		{
			int val = Value;
			if (val == Minimum)
				return;
			--val;
			txtNumVal.Text = val.ToString();
		}

		void OnUpClick(object sender, RoutedEventArgs e)
		{
			int val = Value;
			if (val == Maximum)
				return;
			++val;
			txtNumVal.Text = val.ToString();
		}
	}
}
