﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Model;
using TeamPomodoro.Core;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for UserDetails.xaml
    /// </summary>
    public partial class UserDetails : Window
    {
        public UserDetails()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (!(Controller.Instance.ValidateUser(userName.Text) &&
                Controller.Instance.ValidatePassword(password.SecurePassword)))
            {
                return;
            }

            try
            {
                Cursor = Cursors.Wait;

                Guid? teamId = null;
                if (teams.SelectedItem != null)
                {
                    teamId = ((Team)teams.SelectedItem).TeamId;
                }

                // TODO: Take care of password handling
                await Controller.Instance.UpdateUser(
                    userName.Text,
                    password.SecurePassword,
                    numUpDown.Number,
                    (bool)chkShowWarning.IsChecked,
                    teamId);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
    }
}