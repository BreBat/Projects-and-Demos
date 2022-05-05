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

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for PageAddMember.xaml
	/// </summary>
	public partial class PageAddMember : Page
	{
		bool fnameFocus = false;
		bool lnameFocus = false;
		bool mnameFocus = false;
		public PageAddMember()
		{
			InitializeComponent();
		}
		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string fName = FNameInput.Text;
			string mName = MinitInput.Text;
			string lName = LNameInput.Text;
			string addr = AddressInput.Text;
			string phone = PhoneInput.Text;

			//Strip phone number of extra symbols
			Stack<int> badIndecies = new Stack<int>();
			for (int i = 0;i<phone.Length;i++)
			{
				if (char.IsPunctuation(phone[i])) badIndecies.Push(i);
			}
			while (badIndecies.Count > 0)
			{
				phone = phone.Remove(badIndecies.Pop(), 1);
			}

			if (fName.Length == 0 || fnameFocus == false)
			{
				showInputWarning("First name is empty.");
				return;
			}
			for (int i = fName.Length-1; i >= 0; i--)
			{
				if (char.IsWhiteSpace(fName[i]) == true) fName = fName.Remove(i, 1);
			}

			if (mName.Length > 1 && mnameFocus == true)
			{
				showInputWarning("Middle initial may not be more than one letter long.");
				return;
			}
			if (mName.Length == 0 || mnameFocus == false)
			{
				mName = "NULL";
			}

			if (lName.Length == 0 || lnameFocus == false)
			{
				showInputWarning("Last name is empty.");
				return;
			}
			for (int i = lName.Length-1; i >= 0; i--)
			{
				if (char.IsWhiteSpace(lName[i]) == true) lName = lName.Remove(i, 1);
			}

			if (addr.Length == 0)
			{
				showInputWarning("Member's address is empty.");
				return;
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

			string query = "INSERT INTO MEMBER(FNAME, MI, LNAME, ADDR, PHONE_NUM) VALUES ('" +
				fName + "','" + mName + "','" + lName + "','" + addr + "','" + phone + "')";

			DBInterface.instance.DatabaseQuery(query);

			Window.GetWindow(this).Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}

		private void MinitInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (mnameFocus == false)
			{
				mnameFocus = true;
				MinitInput.Text = "";
			}
		}

		private void LNameInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (lnameFocus == false)
			{
				lnameFocus = true;
				LNameInput.Text = "";
			}
		}

		private void FNameInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (fnameFocus == false)
			{
				fnameFocus = true;
				FNameInput.Text = "";
			}
		}

		private void MinitInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (MinitInput.Text.Length > 1 && mnameFocus == true) MinitInput.Foreground = Brushes.Red;
			else MinitInput.Foreground = Brushes.Black;
		}
	}
}
