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
	/// Interaction logic for booksAndCopies.xaml
	/// </summary>
	public partial class PageMWBooksIndex : Page
	{
		public PageMWBooksIndex()
		{
			InitializeComponent();
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (App.Current.Resources["SQLsetup"].Equals(true))
			{
				SortBookTableLname.IsEnabled = true;
				SortBookTableTitle.IsEnabled = true;
				BooksTable.IsEnabled = true;
				CopiesTable.IsEnabled = true;
			}
		}

		private void SortBookTableLname_Click(object sender, RoutedEventArgs e)
		{
			DataTable lname = DBInterface.instance.DatabaseQuery("SELECT * FROM BOOK ORDER BY AUTH_LNAME ASC");
			lname.Columns[0].ColumnName = "ISBN";
			lname.Columns[1].ColumnName = "Title";
			lname.Columns[2].ColumnName = "Author: First name";
			lname.Columns[3].ColumnName = "Last name";
			lname.Columns[4].ColumnName = "M Initial";
			lname.Columns[5].ColumnName = "Section";
			lname.Columns[6].ColumnName = "Call Number";
			BooksTable.DataContext = lname.DefaultView;
		}

		private void SortBookTableTitle_Click(object sender, RoutedEventArgs e)
		{
			DataTable title = DBInterface.instance.DatabaseQuery("SELECT * FROM BOOK ORDER BY TITLE");
			title.Columns[0].ColumnName = "ISBN";
			title.Columns[1].ColumnName = "Title";
			title.Columns[2].ColumnName = "Author: First name";
			title.Columns[3].ColumnName = "Last name";
			title.Columns[4].ColumnName = "M Initial";
			title.Columns[5].ColumnName = "Section";
			title.Columns[6].ColumnName = "Call Number";
			BooksTable.DataContext = title.DefaultView;
		}

		private void BooksTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataRowView row = (DataRowView)BooksTable.SelectedItem;
			if (BooksTable.SelectedItem == null) return; //If we clicked empty space, don't do this
			string ISBN = row[0].ToString();

			string copyQuery = "SELECT DISTINCT COP.BOOK_COPY_ID, B.TITLE, COP.CONDITION, " +
				"(SELECT CASE " +
					"WHEN(EXISTS(SELECT * FROM BOOK_CHECKOUT BCO WHERE BCO.BOOK_COPY_ID = COP.BOOK_COPY_ID))" +
					"THEN 'Checked Out' " +
					"ELSE 'Checked In' END) AS CHECK_STATUS " +
					"FROM BOOK_COPY COP, BOOK B " +
					"WHERE '" + ISBN + "' = B.ISBN AND B.ISBN = COP.ISBN";
			DataTable copyTable = DBInterface.instance.DatabaseQuery(copyQuery);
			CopiesTable.DataContext = copyTable.DefaultView;
		}
	}
}
