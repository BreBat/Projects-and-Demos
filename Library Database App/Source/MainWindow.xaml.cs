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
using System.Data;
using System.Data.SqlClient;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			App.Current.Resources.Add("SQLsetup", false);

			InitializeComponent();

			pageFrame.Content = new PageMWBooksIndex();
		}
		private void Window_Activated(object sender, EventArgs e)
		{
			if (App.Current.Resources["SQLsetup"].Equals(false))
			{
				if (DBInterface.instance.validSetup() == true)
				{
					//Once DB is connected, enable all features
					App.Current.Resources["SQLsetup"] = true;
					AddBook.IsEnabled = true;
					AddCopy.IsEnabled = true;
					AddMember.IsEnabled = true;
					RmBook.IsEnabled = true;
					RmCopy.IsEnabled = true;
					RemoveMember.IsEnabled = true;
					updateAddr.IsEnabled = true;
					updatePhone.IsEnabled = true;
					updateCond.IsEnabled = true;
					copyInput.IsEnabled = true;
					memberInput.IsEnabled = true;
					checkIn.IsEnabled = true;
					checkOut.IsEnabled = true;
					pageFrame.Refresh();
				}
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{

		}

		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}

		//Utility Method for testing
		private void printDataTable(DataTable data)
		{
			string result = "";
			for (int i = 0; i < data.Rows.Count; i++)
			{
				for (int j = 0; j < data.Columns.Count; j++)
				{
					result += data.Rows[i].ItemArray[j].ToString() + " ";
				}
				result += "\n\n";
			}
			MessageBox.Show(result);
		}

		//FILE MENU OPTIONS////////////////////////////////////////////////////
		private void FileExit_Click(object sender, RoutedEventArgs e)
		{
			App.Current.Shutdown();
		}

		//MANAGE MENU OPTIONS//////////////////////////////////////////////////
		private void SetSQLServer_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBServerSetWindow SQLsetWindow = new DBServerSetWindow();
			SQLsetWindow.Show();
		}

		private void AddBook_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow addBookWindow = new DBupdateWindow();
			addBookWindow.updateType.Content = new PageAddBook();
			addBookWindow.Title = "Add book";
			addBookWindow.Show();
		}

		private void AddCopy_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow addCopyWindow = new DBupdateWindow();
			addCopyWindow.updateType.Content = new PageAddCopy();
			addCopyWindow.Title = "Add book copy";
			addCopyWindow.Show();
		}

		private void AddMember_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow addMemberWindow = new DBupdateWindow();
			addMemberWindow.updateType.Content = new PageAddMember();
			addMemberWindow.Title = "Add member";
			addMemberWindow.Show();
		}

		private void RmBook_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow RemBookWindow = new DBupdateWindow();
			RemBookWindow.updateType.Content = new PageRemBook();
			RemBookWindow.Title = "Remove book";
			RemBookWindow.Show();
		}

		private void RmCopy_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow RemCopyWindow = new DBupdateWindow();
			RemCopyWindow.updateType.Content = new PageRemCopy();
			RemCopyWindow.Title = "Remove book copy";
			RemCopyWindow.Show();
		}

		private void RemoveMember_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow RemMemberWindow = new DBupdateWindow();
			RemMemberWindow.updateType.Content = new PageRemMember();
			RemMemberWindow.Title = "Remove member";
			RemMemberWindow.Show();
		}
		private void updatePhone_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow UpdPhoneWindow = new DBupdateWindow();
			UpdPhoneWindow.updateType.Content = new PageUpdPhone();
			UpdPhoneWindow.Title = "Change phone number";
			UpdPhoneWindow.Show();
		}

		private void updateAddr_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow UpdAddrWindow = new DBupdateWindow();
			UpdAddrWindow.updateType.Content = new PageUpdAddress();
			UpdAddrWindow.Title = "Change address";
			UpdAddrWindow.Show();
		}

		private void updateCond_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.IsEnabled = false;
			DBupdateWindow UpdCondWindow = new DBupdateWindow();
			UpdCondWindow.updateType.Content = new PageUpdCondition();
			UpdCondWindow.Title = "Change book condition";
			UpdCondWindow.Show();
		}

		//HELP MENU OPTIONS////////////////////////////////////////////////////

		private void About_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("CIS 421 Team Project Interface\nBrendan Batchelor\nAlexander Rosati", 
				"About", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void Greytip_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("The program must be told the name of the local SQL server. Use " +
				"\"Manage/Set SQL server name.\"", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		//NAVIGATION BUTTONS///////////////////////////////////////////////////

		private void NavBookIndex_Click(object sender, RoutedEventArgs e)
		{
			pageFrame.Content = new PageMWBooksIndex();
			NavMemberLookup.BorderThickness = new Thickness(1);
			NavBookIndex.BorderThickness = new Thickness(1,1,1,0);
		}

		private void NavMemberLookup_Click(object sender, RoutedEventArgs e)
		{
			pageFrame.Content = new PageMWMemberLookup();
			NavMemberLookup.BorderThickness = new Thickness(1, 1, 1, 0);
			NavBookIndex.BorderThickness = new Thickness(1);
		}

		private bool checkInOutValidation(string memID, string copyID)
		{
			if (copyID.Length != 4)
			{
				showInputWarning("Book Copy ID must be 4 digits long.");
				return false;
			}
			for (int i = 0; i < copyID.Length; i++)
			{
				if (char.IsDigit(copyID[i]) == false)
				{
					showInputWarning("Book Copy ID must be numeric.");
					return false;
				}
			}

			string copyCheck = "SELECT * FROM BOOK_COPY WHERE BOOK_COPY_ID = '" + copyID + "'";
			DataTable copy = DBInterface.instance.DatabaseQuery(copyCheck);
			if (copy.Rows.Count == 0)
			{
				showInputWarning("Book Copy " + copyID + " does not exist.");
				return false;
			}


			if (memID.Length != 4)
			{
				showInputWarning("Member ID must be 4 digits long.");
				return false;
			}
			for (int i = 0; i < memID.Length; i++)
			{
				if (char.IsDigit(memID[i]) == false)
				{
					showInputWarning("Member ID must be numeric.");
					return false;
				}
			}

			string memberCheck = "SELECT * FROM MEMBER WHERE MEM_ID = '" + memID + "'";
			DataTable member = DBInterface.instance.DatabaseQuery(memberCheck);
			if (member.Rows.Count == 0)
			{
				showInputWarning("Member " + memID + " does not exist.");
				return false;
			}

			return true;
		}

		private void checkOut_Click(object sender, RoutedEventArgs e)
		{
			string memID = memberInput.Text;
			string copyID = copyInput.Text;
			if (checkInOutValidation(memID, copyID) == false) return;

			string check = "SELECT * FROM BOOK_CHECKOUT WHERE BOOK_COPY_ID = '" + copyID + "'";
			DataTable checkReturn = DBInterface.instance.DatabaseQuery(check);

			if (checkReturn.Rows.Count == 0)
			{
				string query = "INSERT INTO BOOK_CHECKOUT(BOOK_COPY_ID, MEM_ID, CHECK_OUT_DATE, DUE_DATE) " +
					"VALUES('" + copyID + "','" + memID + "', GETDATE(), DATEADD(DAY, 14, GETDATE()))";

				DBInterface.instance.DatabaseQuery(query);

				string getDates = "SELECT CHECK_OUT_DATE, DUE_DATE FROM BOOK_CHECKOUT WHERE " +
					"BOOK_COPY_ID = '" + copyID + "' AND MEM_ID = '" + memID + "'";
				DataTable dates = DBInterface.instance.DatabaseQuery(getDates);

				string checkout = dates.Rows[0].ItemArray[0].ToString();
				string due = dates.Rows[0].ItemArray[1].ToString();

				for (int i = 0; i < 12; i++) //Strip time off of results
				{
					checkout = checkout.Remove(checkout.Length-1, 1);
					due = due.Remove(due.Length-1, 1);
				}

				MessageBox.Show("Checked out: " + checkout + "\nDue: " + due, 
					"Book Checkout", MessageBoxButton.OK, MessageBoxImage.Information);
				memberInput.Text = "";
				copyInput.Text = "";
			}
			else
			{
				showInputWarning("Book " + copyID + " is already checked out.");
			}
		}

		private void checkIn_Click(object sender, RoutedEventArgs e)
		{
			string memID = memberInput.Text;
			string copyID = copyInput.Text;
			if (checkInOutValidation(memID, copyID) == false) return;

			string check = "SELECT BOOK_COPY_ID, MEM_ID FROM BOOK_CHECKOUT WHERE BOOK_COPY_ID = '" + copyID + "'";
			DataTable checkReturn = DBInterface.instance.DatabaseQuery(check);

			if (checkReturn.Rows.Count == 0)
			{
				showInputWarning("Book " + copyID + " is already checked in.");
			}
			else if (checkReturn.Rows[0].ItemArray[1].ToString() != memID)
			{
				showInputWarning("Book " + copyID + " is not checked out by member " + memID + ".");
			}
			else
			{
				string query = "DELETE FROM BOOK_CHECKOUT WHERE BOOK_COPY_ID = '" + copyID + "'";

				DBInterface.instance.DatabaseQuery(query);

				MessageBox.Show("Book copy " + copyID + " has been checked in and is recorded as available for use by other members.",
					"Book Checkin", MessageBoxButton.OK, MessageBoxImage.Information);
				memberInput.Text = "";
				copyInput.Text = "";
			}
		}
	}
}
