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
	/// Interaction logic for memberLookup.xaml
	/// </summary>
	public partial class PageMWMemberLookup : Page
	{
		public PageMWMemberLookup()
		{
			InitializeComponent();
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (App.Current.Resources["SQLsetup"].Equals(true))
			{
				MembersTable.IsEnabled = true;
				DataTable members = DBInterface.instance.DatabaseQuery("SELECT " +
				"M.FNAME, M.MI, M.LNAME, B.TITLE, CAST(BCO.DUE_DATE AS varchar(10)), M.PHONE_NUM, M.ADDR, " +
				"0.05 * DATEDIFF(DAY, BCO.DUE_DATE, GETDATE()) AS AMOUNT_DUE " +
				"FROM MEMBER M, BOOK B, BOOK_CHECKOUT BCO, BOOK_COPY BC " +
				"WHERE BCO.BOOK_COPY_ID = BC.BOOK_COPY_ID AND BC.ISBN = B.ISBN " +
				"AND BCO.MEM_ID = M.MEM_ID AND DATEDIFF(DAY, BCO.DUE_DATE, GETDATE()) >= 1");
				
				members.Columns[0].ColumnName = "First Name";
				members.Columns[1].ColumnName = "Initial";
				members.Columns[2].ColumnName = "Last Name";
				members.Columns[3].ColumnName = "Book Title";
				members.Columns[4].ColumnName = "Due Date";
				members.Columns[5].ColumnName = "Phone Number";
				members.Columns[6].ColumnName = "Address";
				members.Columns[7].ColumnName = "Amount Due";

				MembersTable.DataContext = members.DefaultView;
			}
		}
	}
}
