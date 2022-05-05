using System.Collections.Generic;
using System.Data;
using CIS476_Project3.Common;

namespace CIS476_Project3.Models
{
	public class CarsModel
	{
		private Dictionary<int, Car> carDB = new Dictionary<int, Car>();
		private int nextID = 0;

		public CarsModel()
		{
			//Sample Data
			AddCar(new Car(14000, true, 1986, "Chevrolet", "Corvette C4", "Silver", 184522));
			AddCar(new Car(18900, true, 1974, "Datsun", "240z", "Orange", 86511));
			AddCar(new Car(27150, true, 2016, "Chevrolet", "SS", "Blue", 25696));
			AddCar(new Car(3000, true, 2001, "Honda", "Civic", "Bronze", 198541));
			AddCar(new Car(35400, false, 2019, "Fiat", "124 Abarth", "Black", 10));
			AddCar(new Car(501000, true, 1974, "Lancia", "Stratos", "Yellow", 984));
		}

		//Add a car to the dictionary
		public void AddCar(Car newCar)
		{
			newCar.SetID(nextID);
			nextID++;

			if (carDB.ContainsKey(newCar.GetID())) return;
			else
			{
				carDB.Add(newCar.GetID(), newCar);
			}
		}

		//intended to be used by modifying the return of getCar
		public void UpdateCar(Car update)
		{
			if (carDB.ContainsKey(update.GetID()))
			{
				carDB[update.GetID()] = update;
			}
			else return; //This should not happen if used correctly
		}

		//Returns true if deletion successful, otherwise false
		public bool RemCar(int ID)
		{
			if (carDB.ContainsKey(ID))
			{
				carDB.Remove(ID);
				return true;
			}
			else return false;
		}

		//Returns null if car not found
		public Car GetCar(int ID)
		{
			if (carDB.ContainsKey(ID))
			{
				return carDB[ID];
			}
			else return null;
		}

		//output all cars in a dataTable object for the view to use
		public DataTable allCarsToDT()
		{
			DataTable output = new DataTable();

			//Define columns and types
			DataColumn id = new DataColumn("ID", System.Type.GetType("System.Int32"));
			DataColumn price = new DataColumn("Price", System.Type.GetType("System.Double"));
			DataColumn used = new DataColumn("Used", System.Type.GetType("System.Boolean"));
			DataColumn year = new DataColumn("Year", System.Type.GetType("System.Int32"));
			DataColumn make = new DataColumn("Make", System.Type.GetType("System.String"));
			DataColumn model = new DataColumn("Model", System.Type.GetType("System.String"));
			DataColumn color = new DataColumn("Color", System.Type.GetType("System.String"));
			DataColumn miles = new DataColumn("Miles", System.Type.GetType("System.Int32"));

			//Insert columns into DT
			output.Columns.Add(id);    //0
			output.Columns.Add(price); //1
			output.Columns.Add(used);  //2
			output.Columns.Add(year);  //3
			output.Columns.Add(make);  //4
			output.Columns.Add(model); //5
			output.Columns.Add(color); //6
			output.Columns.Add(miles); //7

			//Populate rows
			foreach (Car c in carDB.Values)
			{
				DataRow row = output.NewRow();
				row["ID"] = c.GetID();
				row["Price"] = c.price;
				row["Used"] = c.used;
				row["Year"] = c.year;
				row["Make"] = c.make;
				row["Model"] = c.model;
				row["Color"] = c.color;
				row["Miles"] = c.miles;

				output.Rows.Add(row);
			}

			return output;
		}
	}
}
