using System.Windows;
using System.Windows.Controls;

using System.Data;

using CIS476_Project3.Controllers;
using CIS476_Project3.Views;

namespace CIS476_Project3.Views.Pages
{
	/// <summary>
	/// Interaction logic for DealerMode.xaml
	/// </summary>
	public partial class DealerMode : Page
    {
		CarsView carsView;
		InventoryControl invControl;

		int applyMode = 0; //0 for add car, 1 for update car. Affects apply button's actions
		int selectedCar = 0; //ID of the currently selected car from the table

		public DealerMode()
		{
			//Load the correct view, model
			carsView = (CarsView)Application.Current.Properties["CarsView"];
			invControl = (InventoryControl)Application.Current.Properties["InventoryControl"];
			InitializeComponent();

			//Pass model data to view
			invControl.loadCarView();

			//Load view data to UI
			cars.DataContext = carsView.getDefaultView();
		}

		//Sets up interface to accept input for a new car
		private void addCar_Click(object sender, RoutedEventArgs e)
		{
			//Enable buttons
			cancel.IsEnabled = true;
			apply.IsEnabled = true;
			applyMode = 0; //Insert mode
			wipeInputFields();

			inputGroup.Header = "Add Car Info";
			inputGroup.IsEnabled = true; //Allow input
		}

		//Sets up interface to accept input for a car update, populates fields using existing info
		private void updCar_Click(object sender, RoutedEventArgs e)
		{
			//Enable buttons
			cancel.IsEnabled = true;
			apply.IsEnabled = true;
			applyMode = 1; //Update mode

			inputGroup.Header = "Update Car Info";
			inputGroup.IsEnabled = true; //Allow input

			//Load existing car data into input fields to make updating easier for user
			DataRowView selectedRow = (DataRowView)cars.SelectedItem;
			usedChk.IsChecked = (bool)selectedRow[2];
			price.Text		  = selectedRow[1].ToString();
			year.Text		  = (string)selectedRow[3].ToString();
			make.Text		  = (string)selectedRow[4].ToString();
			mod.Text		  = (string)selectedRow[5].ToString();
			miles.Text		  = (string)selectedRow[7].ToString();
			color.Text		  = (string)selectedRow[6].ToString();
		}

		//Remove selected (in table) car from the car database
		//Unlike the other button methods this does not defer to the apply button
		private void remCar_Click(object sender, RoutedEventArgs e)
		{
			if (cars.SelectedItem != null)
			{
				invControl.remCar(selectedCar);
				refreshCarDataGrid();
			}
		}

		//Queries the inventory controller using user inputs to attempt add/update operations
		private void apply_Click(object sender, RoutedEventArgs e)
		{
			bool success = false;
			bool used = (bool)usedChk.IsChecked;

			if (applyMode == 0) //Insert mode
			{
				//Call controller to Push the new addition. this may not do anything if info is entered incorrectly
				success = invControl.addCar(used,
											price.Text,
											year.Text,
											make.Text,
											mod.Text,
											miles.Text,
											color.Text);
				if (success) refreshCarDataGrid();
				else MessageBox.Show("Addition failed. Ensure that all fields are entered correctly and not empty.");
			}

			else if (applyMode == 1) //Update mode
			{
				if (cars.SelectedItem != null)
				{
					//Call controller to Push the update. this may not do anything if info is entered incorrectly
					success = invControl.updateCar(selectedCar, used,
														 price.Text,
														 year.Text,
														 make.Text,
														 mod.Text,
														 miles.Text,
														 color.Text);
					if (success) refreshCarDataGrid();
					else MessageBox.Show("Update failed. Ensure that all fields are entered correctly and not empty.");
				}
			}

			else MessageBox.Show("Error: Selection mode not set. This shouldn't happen." +
										" Try clicking [Add car] or [Update car] agian.");

			//Clean up if the operation worked
			if (success)
			{
				//Disable input group
				cancel.IsEnabled = false;
				apply.IsEnabled = false;
				inputGroup.Header = "";
				inputGroup.IsEnabled = false;
				wipeInputFields();
			}
		}

		//Backs out of an add/update operation
		private void cancel_Click(object sender, RoutedEventArgs e)
		{
			//Disable input group
			cancel.IsEnabled = false;
			apply.IsEnabled = false;
			inputGroup.Header = "";
			inputGroup.IsEnabled = false;

			wipeInputFields();
		}

		//Maintains info on which car is selected in the UI
		private void cars_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			if (cars.SelectedItem != null)
			{
				//If something is selected, un-gray the remove/update buttons
				updCar.IsEnabled = true;
				remCar.IsEnabled = true;
				//And keep track of what is selected
				DataRowView selectedRow = (DataRowView)cars.SelectedItem;
				selectedCar = (int)selectedRow[0];
			}
			else
			{
				//If deselected, gray-out the update/remove buttons
				updCar.IsEnabled = false;
				remCar.IsEnabled = false;
			}
		}

		//Clears text from input fields to make things look nicer
		private void wipeInputFields()
		{
			usedChk.IsChecked = true;
			price.Text = "";
			year.Text = "";
			make.Text = "";
			mod.Text = "";
			miles.Text = "";
			color.Text = "";
		}

		//Helper function to get the UI to show changes to the car table
		private void refreshCarDataGrid()
		{
			//Flash the dataContext to trigger a sourceChange in the datagrid
			cars.DataContext = null;
			cars.DataContext = carsView.getDefaultView();
		}
	}
}
