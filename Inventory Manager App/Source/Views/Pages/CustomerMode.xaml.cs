using System.Windows;
using System.Windows.Controls;
using System.Data;

using CIS476_Project3.Views;
using CIS476_Project3.Controllers;

namespace CIS476_Project3.Views.Pages
{
	/// <summary>
	/// Interaction logic for CustomerMode.xaml
	/// </summary>
	public partial class CustomerMode : Page
	{
		CarsView carsView;
		SearchControl searchControl;
		public CustomerMode()
		{
			//Load the correct view, model
			carsView = (CarsView)Application.Current.Properties["CarsView"];
			searchControl = (SearchControl)Application.Current.Properties["SearchControl"];
			InitializeComponent();

			//Load view data to UI
			cars.DataContext = carsView.getDefaultView();
		}

		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			//Execute through search controller
			DataTable result = searchControl.searchCars(priceMin.Text, priceMax.Text,
														yearMin.Text, yearMax.Text,
														milesMin.Text, milesMax.Text,
														make.Text, model.Text, color.Text,
														condition.Text);
			//Load filtered set of cars into UI table
			cars.DataContext = result.DefaultView;
		}
	}
}
