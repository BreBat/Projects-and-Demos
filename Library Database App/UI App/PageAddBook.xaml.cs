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
	/// Interaction logic for PageAddBook.xaml
	/// </summary>
	public partial class PageAddBook : Page
	{
		bool FNameFirstFocus = false; // These become true after
		bool MinitFirstFocus = false; // each textbox is focused 
		bool LNameFirstFocus = false; // for the first time

		public PageAddBook()
		{
			InitializeComponent();
		}
		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}

		private void showInputWarning(string warning)
		{
			MessageBox.Show(warning, "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
		}

		private int wordCount(string word)
		{
			int wc = 0;
			char previous = ' ';

			for (int i = 0; i < word.Length; i++)
			{
				if (char.IsWhiteSpace(previous) &&  (char.IsLetterOrDigit(word[i]) || char.IsPunctuation(word[i])))
				{
					wc++;
				}
				previous = word[i];
			}
			return wc;
		}


		private void OKbtn_Click(object sender, RoutedEventArgs e)
		{
			string ISBN = ISBNinput.Text;
			string title = TitleInput.Text;
			string fName = FNameInput.Text;
			string mInit = MinitInput.Text;
			string lName = LNameInput.Text;
			string section = SectionInput.Text;
			string callNum = "NULL";

			//ISBN
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

			//Check preemptively if this ISBN is already present
			string isbnQuery = "SELECT CASE WHEN (EXISTS (SELECT * FROM BOOK WHERE '" + ISBN + "' = BOOK.ISBN)) THEN 'taken' ELSE 'free' END";
			DataTable isbnChecker = DBInterface.instance.DatabaseQuery(isbnQuery);
			
			if (isbnChecker.Rows[0].ItemArray[0].ToString() == "taken")
			{
				showInputWarning("ISBN " + ISBN + " already exists in library system.");
				return;
			}

			///Title
			if (title.Length == 0 || title.Length > 150)
			{
				showInputWarning("Title must be between 1-150 characters long.");
				return;
			}

			//First name
			if (fName.Length == 0 || FNameFirstFocus == false)
			{
				showInputWarning("First name is empty.");
				return;
			}
			for (int i = fName.Length - 1; i >= 0; i--)
			{
				if (char.IsWhiteSpace(fName[i]) == true) fName = fName.Remove(i, 1);
			}

			//Initial
			if (mInit.Length > 1 && MinitFirstFocus == true)
			{
				showInputWarning("Middle initial may not be more than one letter long.");
				return;
			}
			if (mInit.Length == 0 || MinitFirstFocus == false) mInit = "NULL";

			//Last name
			if (lName.Length == 0 || LNameFirstFocus == false)
			{
				showInputWarning("Last name is empty.");
				return;
			}
			for (int i = lName.Length - 1; i >= 0; i--)
			{
				if (char.IsWhiteSpace(lName[i]) == true) lName = lName.Remove(i, 1);
			}

			//Section
			section = section.ToUpper();

			//Massive amount of call number verification begins here
			if (CallNumInput.IsEnabled == true)
			{
				callNum = CallNumInput.Text;

				//Check if empty
				if (callNum.Length == 0)
				{
					showInputWarning("Call number is empty.");
					return;
				}
				//Check if two words
				if (wordCount(callNum) != 2)
				{
					showInputWarning("Call number must be two words.");
					return;
				}
				//Check for adjacent periods
				for (int i = 1; i < callNum.Length; i++)
				{
					if (char.IsPunctuation(callNum[i]) && char.IsPunctuation(callNum[i - 1]))
					{
						showInputWarning("Call number may not have two adjacent periods.");
						return;
					}
				}
				//strip leading spaces
				while (callNum.Length > 0 && char.IsWhiteSpace(callNum[0]))
				{
					callNum = callNum.Remove(0, 1);
				}
				//Check if empty again
				if (callNum.Length == 0)
				{
					showInputWarning("Call number is empty.");
					return;
				}
				//Check for leading period
				if (char.IsPunctuation(callNum[0]))
				{
					showInputWarning("Call number may not start with period.");
					return;
				}
				//strip trailing spaces
				while (callNum.Length > 0 && char.IsWhiteSpace(callNum[callNum.Length - 1]))
				{
					callNum = callNum.Remove((callNum.Length - 1), 1);
				}
				//determine bounds of first word
				int middleSpace = 0; //index
				for (int i = 0; i < callNum.Length; i++)
				{
					middleSpace = i;
					if (char.IsWhiteSpace(callNum[i])) break;
				}
				//verify first word
				for (int i = 0; i < middleSpace; i++)
				{
					if (char.IsDigit(callNum[i]) == false && char.IsPunctuation(callNum[i]) == false)
					{
						showInputWarning("Call number's first part must be numeric with periods.");
						return;
					}
				}
				//Strip extra middle spaces
				while (middleSpace != callNum.Length - 1 && char.IsWhiteSpace(callNum[middleSpace + 1]))
				{
					callNum = callNum.Remove(middleSpace + 1, 1);
				}
				//verify second word
				for (int i = middleSpace + 1; i < callNum.Length; i++)
				{
					if (char.IsLetter(callNum[i]) == false)
					{
						showInputWarning("Call number's second part may only contain letters.");
						return;
					}
				}
				//verify second word length
				if (callNum.Length - (middleSpace + 1) > 3 || callNum.Length - (middleSpace + 1) < 2)
				{
					showInputWarning("Call number's second part must be either 2 or 3 characters long.");
					return;
				}
				//Call number is valid
				callNum = callNum.Insert(callNum.Length, "'");
				callNum = callNum.Insert(0, "'");
			}

			string update = "INSERT INTO BOOK(ISBN, TITLE, AUTH_FNAME, AUTH_LNAME, AUTH_MI, SECTION, CALL_NUM) " +
					"VALUES('" + ISBN + "','" + title + "','" + fName + "','" + lName + "',";

			if (mInit == "NULL") update += mInit + ",'";
			else update += "'" + mInit + "','";

			update += section + "'," + callNum + ")";

			DBInterface.instance.DatabaseQuery(update);

			Window.GetWindow(this).Close();
		}

		private void CallNumCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			if (CallNumInput != null && CallNumLabel != null) //just to be careful
			{
				CallNumInput.IsEnabled = true;
				CallNumLabel.Foreground = Brushes.Black; 
			}
		}

		private void CallNumCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (CallNumInput != null && CallNumLabel != null) //Avoid nullptr at page init
			{
				CallNumInput.IsEnabled = false;
				CallNumLabel.Foreground = Brushes.DarkGray;
				CallNumInput.Text = "";
			}
		}

		private void FNameInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (FNameFirstFocus == false) FNameInput.Text = "";
			FNameFirstFocus = true;
			
		}

		private void MinitInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (MinitFirstFocus == false) MinitInput.Text = "";
			MinitFirstFocus = true;
		}

		private void LNameInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (LNameFirstFocus == false) LNameInput.Text = "";
			LNameFirstFocus = true;
		}

		private void MinitInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (MinitInput.Text.Length > 1 && MinitFirstFocus == true) MinitInput.Foreground = Brushes.Red;
			else MinitInput.Foreground = Brushes.Black;
		}
	}
}
