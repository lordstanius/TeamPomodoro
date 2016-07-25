using System.Windows;
using System.Windows.Controls;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for UpDownControl.xaml
    /// </summary>
    public partial class UpDownControl : UserControl
    {
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number",
            typeof(int),
            typeof(UpDownControl),
            new PropertyMetadata(24));

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
                return (int)GetValue(NumberProperty);
            }
            set
            {
                if (value > Maximum || value < Minimum)
                {
                    return;
                }

                SetValue(NumberProperty, value);
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
            SetValue(NumberProperty, val);
        }

        private void OnUpClick(object sender, RoutedEventArgs e)
        {
            int val = Number;
            if (val == Maximum)
            {
                return;
            }

            ++val;
            SetValue(NumberProperty, val);
        }
    }
}
