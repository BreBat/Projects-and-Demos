using System.Data;

namespace CIS476_Project3.Views
{
	//View for handling tables of cars, used by both customer and dealer views
	public class CarsView
	{
		private DataTable carsDB;
		private bool DTsetup = false;

		public CarsView()
		{
			
		}

		//Load a datatable of cars to be used in view with Car datagrids
		public void loadCars(DataTable cars)
		{
			carsDB = cars;
			DTsetup = true;
		}

		//Get the car DT's defaultView to be loaded into a datagrid UI element
		public DataView getDefaultView()
		{
			if (DTsetup) return carsDB.DefaultView;
			else return null;
		}
	}
}
