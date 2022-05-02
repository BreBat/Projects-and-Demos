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
	/// Interaction logic for PageUpdPhone.xaml
	/// </summary>
	public partial class PageUpdPhone : Page
	{
		public PageUpdPhone()
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
			string phone = phoneInput.Text;

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

			if (phone.Length == 0)
			{
				showInputWarning("Phone number is empty.");
				return;
			}

			if (phone.Length == 10)
			{
				//Numeric check
				for (int i = 0; i < phone.Length; i++)
				{
					if (char.IsDigit(phone[i]) == false)
					{
						showInputWarning("Phone number must be numeric.");
						return;
					}
				}
				//Integrity checks
				if (phone[0] == '1')
				{
					showInputWarning("Phone number's first digit must be 2-9.");
					return;
				}
				if (phone[3] == '1')
				{
					showInputWarning("Phone number's fourth digit must be 2-9.");
					return;
				}
				phone = phone.Insert(6, ".");
				phone = phone.Insert(3, ".");
			}
			else
			{
				showInputWarning("Phone number must be 10 digits.");
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
				string query = "UPDATE MEMBER SET PHONE_NUM = '" + phone + "' WHERE MEM_ID = '" + memID + "'";
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
