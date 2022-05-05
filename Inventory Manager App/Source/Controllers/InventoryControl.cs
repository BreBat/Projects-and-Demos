using CIS476_Project3.Common;
using CIS476_Project3.Models;
using CIS476_Project3.Views;

namespace CIS476_Project3.Controllers
{
    class InventoryControl
    {
		private CarsView targetCarsView;
		private CarsModel targetCarsModel;

		public InventoryControl(CarsView targetCV, CarsModel targetCM)
		{
			targetCarsModel = targetCM;
			targetCarsView = targetCV;
		}

		//Passes datatable of all cars from the model to the view
		public void loadCarView()
		{
			targetCarsView.loadCars(targetCarsModel.allCarsToDT());
		}

		//Updates a car in the model using user input. Returns false if input is invalid
		public bool updateCar(int ID, bool used, string price, string year, string make, string model, string miles, string color)
		{
			if (validityCheck(price, year, make, model, miles, color) == false) return false;

			//Convert input strings to values
			double numCost = double.Parse(price);
			int numYear = int.Parse(year);
			int numMi = int.Parse(miles);

			Car newCar = new Car(numCost, used, numYear, make, model, color, numMi);
			newCar.SetID(ID); //Keep ID consistent

			//Execute the update
			targetCarsModel.UpdateCar(newCar);

			//Reload the new info to the view
			loadCarView();
			return true;
		}

		//Adds a car to the model using user input. Returns false if input is invalid
		public bool addCar(bool used, string price, string year, string make, string model, string miles, string color)
		{
			if (validityCheck(price, year, make, model, miles, color) == false) return false;

			//Convert input strings to values
			double numCost = double.Parse(price);
			int numYear = int.Parse(year);
			int numMi = int.Parse(miles);

			Car newCar = new Car(numCost, used, numYear, make, model, color, numMi);

			//Execute the update
			targetCarsModel.AddCar(newCar);

			//Reload the new info to the view
			loadCarView();
			return true;
		}

		//Removes a car from the model using user input.
		public void remCar(int ID)
		{
			targetCarsModel.RemCar(ID);
			loadCarView();
		}

		//Checks user input for invalid items, returns false if found
		private bool validityCheck(string price, string year, string make, string model, string miles, string color)
		{
			if (price.Length == 0) return false;
			if (year.Length == 0) return false;
			if (make.Length == 0) return false;
			if (model.Length == 0) return false;
			if (miles.Length == 0) return false;
			if (color.Length == 0) return false;

			foreach (char c in price)
			{
				if (char.IsDigit(c) == false && c!='.') return false;
			}
			foreach (char c in year)
			{
				if (char.IsDigit(c) == false) return false;
			}
			foreach (char c in miles)
			{
				if (char.IsDigit(c) == false) return false;
			}

			double numCost = double.Parse(price);
			int numYear = int.Parse(year);
			int numMi = int.Parse(miles);

			if (numCost < 1.0) return false;
			if (numYear < 1890) return false;
			if (make.Length < 1) return false;
			if (model.Length < 1) return false;
			if (numMi < 0) return false;
			if (color.Length < 1) return false;
			return true;
		}
    }
}
