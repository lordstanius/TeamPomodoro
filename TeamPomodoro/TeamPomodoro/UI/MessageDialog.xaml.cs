using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using NLog;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        private MessageDialog(string message, string caption, bool isYesNo, bool showCancel, bool isError)
        {
            InitializeComponent();

            imgError.Visibility = isError ? Visibility.Visible : Visibility.Collapsed;
            if (isYesNo)
            {
                btnOk.Content = Strings.TxtYes;
                btnCancel.Content = Strings.TxtNo;
            }
            else
            {
                btnCancel.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;
                btnOk.Content = Strings.TxtOk;
                btnCancel.Content = Strings.TxtCancel;
            }

            Title = caption ?? Strings.TxtTeamPomodoro;
            lMessage.Text = message;
        }

        public static bool Show(string message, string caption, bool isYesNo, bool showCancel, bool isError)
        {
            var msg = new MessageDialog(message, caption, isYesNo, showCancel, isError)
            {
                Owner = Controller.Instance.Main,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            double width = msg.imgError.Visibility == Visibility.Visible ? 260.0 : 304.0;
            msg.lMessage.Measure(new Size(width, double.PositiveInfinity));
            msg.Height = msg.lMessage.DesiredSize.Height + 120;

            return (bool)msg.ShowDialog();
        }

        public static bool Show(string message, string caption = null, bool showCancel = false)
        {
            if (caption == null)
            {
                caption = Strings.TxtInfo;
            }

            return Show(message, caption, false, showCancel, false);
        }

        public static bool ShowYesNo(string message, string caption = null)
        {
            return Show(message, caption, true, true, false);
        }

        public static bool ShowError(Exception ex, string message, string caption = null, bool showCancel = false)
        {
            if (caption == null)
            {
                caption = Strings.TxtError;
            }

            Logger log = LogManager.GetLogger(string.Empty);
            log.Error(ex, ex.Message, null);

            return Show(string.Format("{0}: {1} {2}", message, ex.Message, Strings.TxtSeeLogForDetails), caption, false, showCancel, true);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
