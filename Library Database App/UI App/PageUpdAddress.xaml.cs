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
	/// Interaction logic for PageUpdAddress.xaml
	/// </summary>
	public partial class PageUpdAddress : Page
	{
		public PageUpdAddress()
		{
			InitializeComponent();
		}
		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}

		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string memID = memInput.Text;
			string addr = addrInput.Text;

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

			if (addr.Length == 0)
			{
				showInputWarning("Member's address is empty.");
				return;
			}

			string checkQuery = "SELECT * FROM MEMBER WHERE MEM_ID = '" + memID + "'";
			DataTable check = DBInterface.instance.DatabaseQuery(checkQuery);

			if (check.Rows.Count == 0)
			{
				showInputWarning("Member " + memID + " not found.");
				return;
			}
			else
			{
				string query = "UPDATE MEMBER SET ADDR = '" + addr + "' WHERE MEM_ID = '" + memID + "'";
				DBInterface.instance.DatabaseQuery(query);
			}

			Window.GetWindow(this).Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}
	}
}
