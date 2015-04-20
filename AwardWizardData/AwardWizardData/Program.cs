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

			SQLStructures[] sql = new SQLStructures[] {
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Award Show.tsv", "AwardShow"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Countries.tsv", "Countries"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - GenreOf.tsv", "GenreOf"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Honor.tsv", "Honor"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Locations.tsv", "Locations"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Movies.tsv", "Movies"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Music.tsv", "Music"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - People.tsv", "People"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Pictures.tsv", "Pictures"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Stage.tsv", "Stage"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Television.tsv", "Television"),
				new SQLStructures (@"/Users/emilychao/Documents/CS/CS411/Data/AwardWizardData/Data Collection - Works.tsv", "Works")
			};

			for (int s = 0; s < sql.Length; s++) {
				DisplayCommands (sql [s]);
			}
		}

		public static void DisplayCommands (SQLStructures sql)
		{
			DataTable data = sql.GetData ();
			Console.WriteLine (sql.tableColumnDictionary [sql.TableName].CreateTableCommand);
			foreach (DataRow row in data.Rows) {
				Console.WriteLine (sql.GetTableInsertCommand (row));
			}
		}
	}
}
