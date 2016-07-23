using System.Windows;
using System.Windows.Controls;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for UpDownControl.xaml
    /// </summary>
    public partial class UpDownControl : UserControl
    {
        public UpDownControl()
        {
            InitializeComponent();

            Minimum = 0;
            Maximum = 100;
        }

        public int Number
        {
            get
            {
                return int.Parse(txtNumVal.Text);
            }
            set
            {
                if (value > Maximum || value < Minimum)
                {
                    return;
                }

                txtNumVal.Text = value.ToString();
            }
        }

        public int Minimum { get; set; }
        public int Maximum { get; set; }

        private void OnDownClick(object sender, RoutedEventArgs e)
        {
            int val = Number;
            if (val == Minimum)
            {
                return;
            }

            --val;
            txtNumVal.Text = val.ToString();
        }

        private void OnUpClick(object sender, RoutedEventArgs e)
        {
            int val = Number;
            if (val == Maximum)
            {
                return;
            }

            ++val;
            txtNumVal.Text = val.ToString();
        }
    }
}
