using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AwardWizardData
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting SQL Conversion");

			SQLStructures sql = new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Honor.tsv", "Honor");

			DisplayCommands (sql);
		}

		public static void DisplayCommands (SQLStructures sql)
		{
			DataTable data = sql.GetData ();
			Console.WriteLine (sql.tableColumnDictionary [sql.TableName].CreateTableCommand);
			foreach (DataRow row in data.Rows) {
				Console.WriteLine (sql.GetTableInsertCommand (row));
			}
			Console.WriteLine ("Finished");
		}
	}
}
