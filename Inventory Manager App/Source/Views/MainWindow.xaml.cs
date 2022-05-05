using System.Windows;

using CIS476_Project3.Models;
using CIS476_Project3.Controllers;
using CIS476_Project3.Views;
using CIS476_Project3.Views.Pages;

namespace CIS476_Project3
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		CarsModel carsModel;
		CarsView carsView;
		LoginControl loginController;
		InventoryControl invController;
		SearchControl searchController;

		public MainWindow()
		{
			//Instantiate our globally-used MVC objects
			carsModel = new CarsModel();
			carsView = new CarsView();
			loginController = new LoginControl();
			invController = new InventoryControl(carsView, carsModel);
			searchController = new SearchControl(carsModel);

			//Save our MVC objects at the app-level so that all windows/pages can use them
			Application.Current.Properties.Add("CarsView", carsView);
			Application.Current.Properties.Add("CarsModel", carsModel);
			Application.Current.Properties.Add("LoginControl", loginController);
			Application.Current.Properties.Add("InventoryControl", invController);
			Application.Current.Properties.Add("SearchControl", searchController);

			InitializeComponent();

			//Pass initial model data to view
			invController.loadCarView();

			//Opening screen is customer view
			currentView.Content = new CustomerMode(); 
		}

		//Called when the window becomes grayed out (or the reverse)
		private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			//If the window was just re-enabled, check if it's because of a new login
			if (Window.GetWindow(this).IsEnabled)
			{
				if (loginController.isLoggedIn)
				{
					//If so, load the dealer options page
					currentView.Content = new DealerMode();
					logout.IsEnabled = true;
					login.IsEnabled = false;
				}
				else
				{
					//Otherwise, we should be on the search page
					currentView.Content = new CustomerMode();
				}
			}
		}

		private void login_Click(object sender, RoutedEventArgs e)
		{
			//Launch a login dialogue and disallow actions on the main window until it resolves
			App.Current.MainWindow.IsEnabled = false;
			Window login = new LoginWindow();
			login.Show();
		}

		private void logout_Click(object sender, RoutedEventArgs e)
		{
			loginController.logout();
	
			//Disable/enable correct UI buttons
			logout.IsEnabled = false;
			login.IsEnabled = true;

			//Exit dealer mode
			currentView.Content = new CustomerMode();
		}
	}
}
