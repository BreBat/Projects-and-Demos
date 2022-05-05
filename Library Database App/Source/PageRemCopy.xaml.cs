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
	/// Interaction logic for PageRemCopy.xaml
	/// </summary>
	public partial class PageRemCopy : Page
	{
		public PageRemCopy()
		{
			InitializeComponent();
		}
		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string copyID = copyInput.Text;

			if (copyID.Length != 4)
			{
				showInputWarning("Book Copy ID must be 4 digits long.");
				return;
			}
			for (int i = 0; i < copyID.Length; i++)
			{
				if (char.IsDigit(copyID[i]) == false)
				{
					showInputWarning("Book Copy ID must be numeric.");
					return;
				}
			}

			string checkQuery = "SELECT * FROM BOOK_COPY WHERE BOOK_COPY_ID = '" + copyID + "'";
			DataTable check = DBInterface.instance.DatabaseQuery(checkQuery);

			string preQuery = "SELECT * FROM BOOK_CHECKOUT WHERE BOOK_COPY_ID = '" + copyID + "'";
			DataTable pre = DBInterface.instance.DatabaseQuery(preQuery);
			if (pre.Rows.Count != 0)
			{
				showInputWarning("May not remove a book copy which is currently checked out.");
				return;
			}
			else if (check.Rows.Count == 0)
			{
				showInputWarning("Book copy " + copyID + " not found.");
				return;
			}
			else
			{
				string titleQuery = "SELECT TITLE FROM BOOK, BOOK_COPY WHERE BOOK_COPY.ISBN = " +
					"BOOK.ISBN AND BOOK_COPY.BOOK_COPY_ID = '" + copyID + "'";
				DataTable titleReturn = DBInterface.instance.DatabaseQuery(titleQuery);
				string title = titleReturn.Rows[0].ItemArray[0].ToString();

				string query = "DELETE FROM BOOK_COPY WHERE BOOK_COPY_ID = '" + copyID + "'";
				DBInterface.instance.DatabaseQuery(query);

				MessageBox.Show("A copy of " + title + " with ID " + copyID + " was removed from the system.",
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
