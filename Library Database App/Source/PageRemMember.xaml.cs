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
	/// Interaction logic for PageRemMember.xaml
	/// </summary>
	public partial class PageRemMember : Page
	{
		public PageRemMember()
		{
			InitializeComponent();
		}
		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}

		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string memID = memInput.Text;

			if (memID.Length != 4)
			{
				showInputWarning("Member ID must be 4 digits long.");
				return;
			}
			for (int i = 0; i < memID.Length; i++) 
			{
				if (char.IsDigit(memID[i]) == false)
				{
					showInputWarning("Member ID must be numeric.");
					return;
				}
			}
			
			string checkQuery = "SELECT * FROM MEMBER WHERE MEM_ID = '" + memID + "'";
			DataTable check = DBInterface.instance.DatabaseQuery(checkQuery);

			string preQuery = "SELECT * FROM BOOK_CHECKOUT WHERE MEM_ID = '" + memID + "'";
			DataTable rentCheck = DBInterface.instance.DatabaseQuery(preQuery);
			if(check.Rows.Count == 0)
			{
				showInputWarning("Member " + memID + " not found.");
				return;
			}
			else if (rentCheck.Rows.Count != 0)
			{
				showInputWarning("May not remove a member with checked out books.");
				return;
			}
			else
			{
				string query = "DELETE FROM MEMBER WHERE MEM_ID = '" + memID + "'";
				DBInterface.instance.DatabaseQuery(query);

				MessageBox.Show("Member " + memID + " was removed from the system.",
					"Removal", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			Window.GetWindow(this).Close();
		}
	}
}
