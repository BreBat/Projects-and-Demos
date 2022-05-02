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
	/// Interaction logic for PageAddCopy.xaml
	/// </summary>
	public partial class PageAddCopy : Page
	{
		public PageAddCopy()
		{
			InitializeComponent();
		}

		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}

		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string ISBN = ISBNinput.Text;
			string cond = ConditionInput.Text.ToUpper();

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

			string update = "INSERT INTO BOOK_COPY(ISBN, CONDITION) VALUES('" + ISBN + "','" + cond + "')";
			try
			{
				DBInterface.instance.DatabaseQuery(update);
			}
			catch
			{
				MessageBox.Show("ISBN not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
			}

			Window.GetWindow(this).Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}
	}
}
