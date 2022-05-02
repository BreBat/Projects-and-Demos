using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp1
{
	class DBInterface
	{
		private static readonly DBInterface singletonDB = new DBInterface();
		private string serverName;
		private bool nameSet = false;

		private DBInterface() { }
		static DBInterface() { }

		public static DBInterface instance {get { return singletonDB; } }

		public bool setDBServer(string name)
		{
			serverName = name;
			SqlConnection pingConn = new SqlConnection("server=" + serverName + ";" +
													"Database=ALMOMANI_PUBLIC_LIBRARY;" +
													"Integrated Security=true");
			try
			{
				pingConn.Open();
			}
			catch
			{
				return nameSet;
			}
			
			pingConn.Close();
			nameSet = true;
			return nameSet;
		}

		public DataTable DatabaseQuery(string query)
		{
			if (nameSet == false)
			{
				return null; //Maybe handle this better
			}
			SqlConnection dbConn = new SqlConnection("server=" + serverName + ";" +
													"Database=ALMOMANI_PUBLIC_LIBRARY;" +
													"Integrated Security=true");
			dbConn.Open();
			SqlCommand testQ = new SqlCommand(query, dbConn);
			SqlDataReader response = testQ.ExecuteReader();
			DataTable result = new DataTable(query);
			result.Load(response);
			dbConn.Close();

			return result;
		}
		public bool validSetup() { return nameSet; }
	}
}
