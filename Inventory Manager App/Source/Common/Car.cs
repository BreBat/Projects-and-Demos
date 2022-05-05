using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS476_Project3.Common
{
	//Defines a car to be stored in the carsModel
	public class Car
	{
		private int ID;
		public double price = 0f;
		public bool used = false;
		public int year = 0;
		public string make = "x";
		public string model = "x";
		public string color = "x";
		public int miles = 0;

		bool IDset = false;

		public Car(double cost, bool preowned, int yr, string mk, string mod, string col, int mi)
		{
			price = cost;
			used = preowned;
			year = yr;
			make = mk;
			model = mod;
			color = col;
			miles = mi;
		}

		public void SetID(int newID)
		{
			if (IDset) return; //Only set once
			else
			{
				IDset = true;
				ID = newID;
			}
		}

		public int GetID() { return ID; }
	}
}
