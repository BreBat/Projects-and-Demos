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
	/// Interaction logic for PageUpdCondition.xaml
	/// </summary>
	public partial class PageUpdCondition : Page
	{
		public PageUpdCondition()
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
			string cond = ConditionInput.Text.ToUpper();

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

			if (check.Rows.Count == 0)
			{
				showInputWarning("Book copy " + copyID + " not found.");
				return;
			}
			else
			{
				string query = "UPDATE BOOK_COPY SET CONDITION = '" + cond + "' WHERE BOOK_COPY_ID = '" + copyID + "'";
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
