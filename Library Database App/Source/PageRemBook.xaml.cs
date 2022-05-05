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

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for PageRemBook.xaml
	/// </summary>
	public partial class PageRemBook : Page
	{
		public PageRemBook()
		{
			InitializeComponent();
		}
		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string ISBN = isbnInput.Text;

			if (ISBN.Length != 10 && ISBN.Length != 13)
			{
				showInputWarning("ISBN may only be 10 or 13 digits.");
				return;
			}
			foreach (char d in ISBN)
			{
				if (char.IsDigit(d) == false)
				{
					showInputWarning("ISBN must be numeric.");
					return;
				}
			}

			string checkQuery = "SELECT TITLE FROM BOOK WHERE ISBN = '" + ISBN + "'";
			DataTable check = DBInterface.instance.DatabaseQuery(checkQuery);

			string preQuery = "SELECT * FROM BOOK_COPY WHERE ISBN = '" + ISBN + "'";
			DataTable pre = DBInterface.instance.DatabaseQuery(preQuery);
			if (pre.Rows.Count != 0)
			{
				showInputWarning("Cannot remove the book because there are copies of it in the system.");
				return;
			}
			else if (check.Rows.Count == 0)
			{
				showInputWarning("The ISBN " + ISBN + " is invalid.");
				return;
			}
			else
			{
				string query = "DELETE FROM BOOK WHERE ISBN = '" + ISBN + "'";
				string title = check.Rows[0].ItemArray[0].ToString();
				DBInterface.instance.DatabaseQuery(query);

				MessageBox.Show(title + " with ISBN " + ISBN + " was removed from the system.",
					"Removal", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			Window.GetWindow(this).Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}
	}
}
