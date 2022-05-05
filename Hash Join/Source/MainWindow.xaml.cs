using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using Microsoft.Win32;

namespace CIS421hw4
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DataTable R1   = new DataTable();
		DataTable R2   = new DataTable();

		public MainWindow()
		{
			InitializeComponent();			
		}

		private DataTable parseInput(string csv)
		{
			DataTable result = new DataTable();

			int numCommas = 0;
			int numRows = 0;
			int numValues = 0;
			List<int> values = new List<int>();
			string currentNum = "";

			//Parse the raw input. Record values and count commas, newlines
			foreach (char c in csv)
			{
				if (char.IsDigit(c)) currentNum += c;
				if (c == ',')
				{
					numCommas++;
					values.Add(int.Parse(currentNum));
					currentNum = "";
				}
				if (c == '\n')
				{
					numRows++;
					values.Add(int.Parse(currentNum));
					currentNum = "";
				}
			}
			numValues = values.Count;
			int numCols = numValues / numRows;

			//Add column definitions to a new datatable
			for (int i = 0; i < numCols; i++)
			{
				DataColumn newCol = new DataColumn();
				newCol.DataType = System.Type.GetType("System.Int32");
				newCol.ColumnName = "R" + (i + 1);
				result.Columns.Add(newCol);
			}

			//Add values to new datatable
			int currentVal = 0;
			for (int i = 0; i < numRows; i++)
			{
				DataRow newRow = result.NewRow();
				foreach (DataColumn col in result.Columns)
				{
					newRow[col.ColumnName] = values[currentVal];
					currentVal++;
				}
				result.Rows.Add(newRow);
			}

			return result;
		}

		private void openR1_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog r1open = new OpenFileDialog();
			r1open.Filter = "Comma Sorted Values (*.csv)|*.csv|Text files(*.txt)|*.txt";
			if (r1open.ShowDialog() == false)
			{
				return; //File not selected
			}
			else
			{
				string raw = File.ReadAllText(r1open.FileName);
				R1file.Text = r1open.SafeFileName; //Show filename on screen
				DataTable newTable = parseInput(raw); //Create data from file
				loadR1(newTable); //Show data on screen
			}
		}

		private void loadR1(DataTable inTable)
		{
			R1 = inTable.Copy();

			//Clear the equijoin attribute options
			R1attrib.Items.Clear();

			for (int i = 0; i < R1.Columns.Count; i++)
			{
				//Set options for equijoin attribute choices in the UI
				R1attrib.Items.Add("R1.a" + (i + 1).ToString());

				//Set the column names for R1's table in the UI
				inTable.Columns[i].ColumnName = "a" + (i + 1).ToString();
				R1.Columns[i].ColumnName = "a" + (i + 1).ToString();
			}

			//Assign R1's tuples to the UI table for R1
			R1Table.DataContext = inTable.DefaultView;

			
			R1Table.Items.Refresh();
		}

		private void openR2_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog r2open = new OpenFileDialog();
			r2open.Filter = "Comma Sorted Values (*.csv)|*.csv|Text files(*.txt)|*.txt";
			if (r2open.ShowDialog() == false)
			{
				return; //File not selected
			}
			else
			{
				string raw = File.ReadAllText(r2open.FileName);
				R2file.Text = r2open.SafeFileName; //Show filename on screen
				DataTable newTable = parseInput(raw); //Create data from file
				loadR2(newTable); //Show data on screen
			}
		}

		private void loadR2(DataTable inTable)
		{
			R2 = inTable.Copy();

			//Clear the equijoin attribute options
			R2attrib.Items.Clear();

			for (int i = 0; i < R2.Columns.Count; i++)
			{
				//Set options for equijoin attribute choices in the UI
				R2attrib.Items.Add("R2.b" + (i + 1).ToString());

				//Set the column names for R2's table in the UI
				inTable.Columns[i].ColumnName = "b" + (i + 1).ToString();
				R2.Columns[i].ColumnName = "b" + (i + 1).ToString();
			}

			//Assign R2's tuples to the UI table for R1
			R2Table.DataContext = inTable.DefaultView;

			R2Table.Items.Refresh();
		}

		private void join_Click(object sender, RoutedEventArgs e)
		{
			//Do nothing if the data is not loaded
			if (R1.IsInitialized == false || R2.IsInitialized == false)
				return;

			//Do nothing if data is empty
			if (R1.Columns.Count == 0 || R2.Columns.Count == 0)
				return;

			//Do nothing if data has no tuples
			if (R1.Rows.Count == 0 || R2.Rows.Count == 0)
				return;

			//Do nothing if we haven't selected equijoin attributes on each side
			if (R1attrib.Text == "" || R2attrib.Text == "")
				return;

			//Determine which attributes to check in each relation
			int R1AttribCheck = int.Parse(R1attrib.Text.Remove(0,4));
			int R2AttribCheck = int.Parse(R2attrib.Text.Remove(0,4));

			DataTable join = hashJoin(R1AttribCheck, R2AttribCheck);

			float selectivityVal = 0.0f;
			selectivityVal = 100 * join.Rows.Count / (R1.Rows.Count * R2.Rows.Count);
			Math.Round(selectivityVal, 2);

			//Display results on UI
			joinTable.DataContext = join.DefaultView;
			selectivity.Text = "Selectivity: " + selectivityVal + "%";
		}
		
		private DataTable hashJoin(int R1a, int R2b)
		{
			DataTable result = new DataTable();

			//Use columns from R1 and R2 to create the join table
			foreach (DataColumn c in R1.Columns)
			{
				DataColumn copy = new DataColumn();
				copy.ColumnName = c.ColumnName;
				copy.DataType = c.DataType;
				result.Columns.Add(copy);
			}
			foreach (DataColumn c in R2.Columns)
			{
				DataColumn copy = new DataColumn();
				copy.ColumnName = c.ColumnName;
				copy.DataType = c.DataType;
				result.Columns.Add(copy);
			}

			Dictionary<int, List<DataRow>> hashTable = new Dictionary<int, List<DataRow>>();
			DataTable relationOne; //Has less rows
			DataTable relationTwo; //Has more rows

			//Determine which table has less rows, assign values accordingly
			if (R1.Rows.Count <= R2.Rows.Count)
			{
				relationOne = R1;
				relationTwo = R2;
			}
			else
			{
				relationOne = R2;
				relationTwo = R1;

				int swap = R1a;
				R1a = R2b;
				R2b = swap;
			}

			int numBuckets = relationOne.Rows.Count * 2; // N

			//Build phase of hash join
			foreach (DataRow currentRow in relationOne.Rows)
			{
				int attribute = int.Parse(currentRow.ItemArray[R1a - 1].ToString());

				int hashval = attribute % numBuckets; // f(k) = k mod N

				//Insert to hashtable
				if (hashTable.ContainsKey(hashval))
				{
					List<DataRow> existingEntry = hashTable[hashval];
					existingEntry.Add(currentRow); //Insert into hashtable (existing key)
				}
				else
				{
					hashTable.Add(hashval, new List<DataRow> { currentRow }); //Insert to hashtable (new key)
				}
			}

			//Probe phase of hash join
			foreach (DataRow currentRow in relationTwo.Rows)
			{
				int attribute = int.Parse(currentRow.ItemArray[R2b - 1].ToString());

				int hashval = attribute % numBuckets; // f(k) = k mod N
				int startHash = hashval; // used for collision handling

				List<DataRow> foundRowList;
				if (hashTable.TryGetValue(hashval, out foundRowList)) //Something has the right hash
				{
					foreach (DataRow foundRow in foundRowList) //See if this hash contains what we want
					{
						if (Equals(foundRow.ItemArray[R1a - 1], attribute)) //Is it the right value?
						{
							//Construct the join result
							DataRow resultTuple = result.NewRow();

							//Populate fields of the new joined tuple
							foreach (DataColumn entry in foundRow.Table.Columns)
							{
								resultTuple[entry.ColumnName] = foundRow[entry.ColumnName];
							}
							foreach (DataColumn entry in currentRow.Table.Columns)
							{
								resultTuple[entry.ColumnName] = currentRow[entry.ColumnName];
							}

							result.Rows.Add(resultTuple); //Add new tuple to join set
						}
					}
				}
			}
			return result;
		}

	} //end of mainwindow
} //end of namespace
					
					