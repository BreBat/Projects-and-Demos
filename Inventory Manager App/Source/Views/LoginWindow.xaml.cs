using System;
using System.Windows;

using CIS476_Project3.Controllers;

namespace CIS476_Project3.Views
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		LoginControl login;
		public LoginWindow()
		{
			//Get the login control object from the app
			login = (LoginControl)App.Current.Properties["LoginControl"];
			InitializeComponent();
		}

		private void Login_Click(object sender, RoutedEventArgs e)
		{
			login.login(user.Text, pass.Text);
			if (login.isLoggedIn == false) //Display an error message for bad logins
			{
				MessageBox.Show("Invalid credentials.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			else this.Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			//Re-enable main window before returning
			App.Current.MainWindow.IsEnabled = true;
		}

		private void cheat_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Login: admin\nPass: 13579", "Demo Login", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
