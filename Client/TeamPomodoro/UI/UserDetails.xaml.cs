﻿using System;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;
using Model;

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

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		async void OnSaveClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.SecurePassword.ToString()))
				return;

			try
			{
				Guid? teamId = null;
				if (cbTeam.SelectedItem != null)
					teamId = ((Team)cbTeam.SelectedItem).TeamId;

				// TODO: Take care of password handling
				DialogResult = await Controller.Instance.AddUser(
					txtUserName.Text,
					"Pass123",
					int.Parse(txtNumVal.Text),
					(bool)chkShowWarning.IsChecked,
					teamId
				);

				if (DialogResult == true)
					return;

				MessageBox.Show(Strings.MsgUserExists, Strings.TxtTeamPomodoro, MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Strings.MsgFailedToRespond, MessageBoxButton.OK, MessageBoxImage.Error);
			}

			DialogResult = null;
		}
	}
}
