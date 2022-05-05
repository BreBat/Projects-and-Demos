namespace CIS476_Project3.Controllers
{
	class LoginControl
	{
		public LoginControl()
		{
			isLoggedIn = false;
		}

		public bool isLoggedIn { get; private set; }

		public void login(string user, string pass)
		{
			//Hardcoded login details for the sake of not complicating this program any further
			if (user == "admin" && pass == "13579")	isLoggedIn = true;
		}

		public void logout()
		{
			isLoggedIn = false;
		}
	}
}
