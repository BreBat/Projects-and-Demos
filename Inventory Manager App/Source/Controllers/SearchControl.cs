using System.Collections.Generic;
using System.Data;

using CIS476_Project3.Models;

namespace CIS476_Project3.Controllers
{
    class SearchControl
    {
		private DataTable masterTable;
		private CarsModel carsModel;
		public SearchControl(CarsModel cm)
		{
			carsModel = cm;
			updateMasterTable();
		}

		//Requests a new cars dataTable from the model
		private void updateMasterTable()
		{
			masterTable = carsModel.allCarsToDT();
		}

		//Generate a filtered car table based on inputs
		public DataTable searchCars(string pricemin, string pricemax,
									string yearmin, string yearmax,
									string milesmin, string milesmax,
									string make, string model,
									string color, string condition)
		{
			updateMasterTable();
			DataTable result = masterTable.Copy();

			//If one of the parameters passed in is empty, do not use its filter
			//Apply all filters by continually using outputs as the next input
			if (color.Length > 0)
			{
				result = searchByColor(color.ToLower(), result);
			}
			if (model.Length > 0)
			{
				result = searchByModel(model.ToLower(), result);
			}
			if (make.Length > 0)
			{
				result = searchByMake(make.ToLower(), result);
			}

			if (validityChecks(pricemin, pricemax, yearmin, yearmax, milesmin, milesmax))
			{
				if (pricemin.Length > 0 && pricemax.Length > 0)
				{
					double min = double.Parse(pricemin);
					double max = double.Parse(pricemax);
					result = filterByPrice(min, max, result);
				}
				if (yearmin.Length > 0 && yearmax.Length > 0)
				{
					int min = int.Parse(yearmin);
					int max = int.Parse(yearmax);
					result = filterByYear(min, max, result);
				}
				if (milesmin.Length > 0 && milesmax.Length > 0)
				{
					int min = int.Parse(milesmin);
					int max = int.Parse(milesmax);
					result = filterByMiles(min, max, result);
				}
			}

			if (condition == "Both") ;//Do nothing
			else if (condition == "Used")result = filterByCondition(true, result);
			else if (condition == "New") result = filterByCondition(false, result);

			return result;
		}

		//Some checks to avoid bad conversions in the searches. False if data is bad
		private bool validityChecks(string price1, string price2,
									string year1, string year2,
									string miles1, string miles2)
		{
			foreach (char c in price1)
			{
				if (char.IsDigit(c) == false && c != '.') return false;
			}
			foreach (char c in price2)
			{
				if (char.IsDigit(c) == false && c != '.') return false;
			}
			foreach (char c in year1)
			{
				if (char.IsDigit(c) == false) return false;
			}
			foreach (char c in year2)
			{
				if (char.IsDigit(c) == false) return false;
			}
			foreach (char c in miles1)
			{
				if (char.IsDigit(c) == false) return false;
			}
			foreach (char c in miles2)
			{
				if (char.IsDigit(c) == false) return false;
			}
			return true;
		}

		//For all below helpers:
		//Removals list is done to separate the detection process from the removing process
		//To prevent removal of elements during foreach loops that concern them

		private DataTable filterByPrice(double min, double max, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				if ((double)row["Price"] < min  || (double)row["Price"] > max)
				{
					removals.Add(row);
				}
			}
			foreach (DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}

		private DataTable filterByYear(int min, int max, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				if ((int)row["Year"] < min || (int)row["Year"] > max)
				{
					removals.Add(row);
				}
			}
			foreach (DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}

		private DataTable filterByCondition(bool used, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				if ((bool)row["Used"] != used)
				{
					removals.Add(row);
				}
			}
			foreach(DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}

		private DataTable filterByMiles(int min, int max, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				if ((int)row["Miles"] < min || (int)row["Miles"] > max)
				{
					removals.Add(row);
				}
			}
			foreach (DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}

		private DataTable searchByMake(string search, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				string lowercaseTarget = ((string)row["Make"]).ToLower();

				if (lowercaseTarget.Contains(search))
				{
					//include
				}
				else removals.Add(row);
			}
			foreach (DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}

		private DataTable searchByModel(string search, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				string lowercaseTarget = ((string)row["Model"]).ToLower();

				if (lowercaseTarget.Contains(search))
				{
					//include
				}
				else removals.Add(row);
			}
			foreach (DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}

		private DataTable searchByColor(string search, DataTable table)
		{
			List<DataRow> removals = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				string lowercaseTarget = ((string)row["Color"]).ToLower();

				if (lowercaseTarget.Contains(search))
				{
					//include
				}
				else removals.Add(row);
			}
			foreach (DataRow row in removals)
			{
				table.Rows.Remove(row);
			}
			return table;
		}
	}
}
