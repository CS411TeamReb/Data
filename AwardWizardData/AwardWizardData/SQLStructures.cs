using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AwardWizardData
{
	public class SQLStructures
	{
		public class SQLTable
		{
			public string Name { get; set; }

			public string[] ColumnNames { get; set; }

			public string CreateTableCommand { get; set; }
		}

		string FilePath;
		public string TableName;
		public Dictionary<string, SQLTable> tableColumnDictionary = new Dictionary<string, SQLTable> () { { "AwardShow", 
				new SQLTable () { 
					Name = "AwardShow", 
					ColumnNames = new string[] { 
						"ShowName", 
						"Description", 
						"Year", 
						"Type", 
						"Criteria", 
						"VotingPanel" 
					},
					CreateTableCommand = "CREATE TABLE AwardShow( ShowName VARCHAR(100) PRIMARY KEY, Description VARCHAR(2000), Year INT, Type VARCHAR(100), Criteria VARCHAR(2000), VotingPanel VARCHAR(2000));"
				}
			}, {
				"Stage",
				new SQLTable () {
					Name = "Stage",
					ColumnNames = new string[] {
						"WorkID",
						"Setting",
						"Title",
						"Iteration",
						"Type",
						"Genre",
						"SongNumber",
						"Year",
						"Theatre",
						"Open",
						"Closed",
						"Previews",
						"Performances",
						"Running"
					},
					CreateTableCommand = "CREATE TABLE Stage( WorkID VARCHAR(50) PRIMARY KEY, Setting VARCHAR(100) NULL, Title VARCHAR(2000) NULL, Iteration INT NULL, Type VARCHAR(100) NULL, Genre VARCHAR(100) NULL, SongNumber INT NULL, YEAR INT NULL, Theatre VARCHAR(300) NULL, Open DATETIME NULL, Closed DATETIME NULL, Previews INT NULL, Performances INT NULL, Running VARCHAR(3) NOT NULL);"
				}
			}, {
				"Music",
				new SQLTable () {
					Name = "Music",
					ColumnNames = new string[] {
						"WorkID",
						"Title",
						"Artist",
						"isSingle",
						"EligibilityYear",
						"Genre",
						"ReleaseYear"
					},
					CreateTableCommand = "CREATE TABLE Music( WorkID VARCHAR(50), Title VARCHAR(2000) NULL, Artist VARCHAR(100), isSingle VARCHAR(3) NOT NULL, EligibilityYear INT NULL, Genre VARCHAR(100) NULL, ReleaseYear INT NULL);"
				}
			}, {
				"People",
				new SQLTable () {
					Name = "People",
					ColumnNames = new string[] {
						"WorkID",
						"Name",
						"PlaceOrigin",
						"Occupation",
						"Gender",
						"Birthdate"
					},
					CreateTableCommand = "CREATE TABLE People(WorkID VARCHAR(50) PRIMARY KEY, Name VARCHAR(200), PlaceOrigin VARCHAR(100) NULL, Occupation VARCHAR(100) NULL, Gender VARCHAR(50) NULL, Birthdate DATETIME NULL);"
				}
			}, {
				"Honor",
				new SQLTable () {
					Name = "Honor",
					ColumnNames = new string[] {
						"AwardID",
						"AwardName",
						"YearGiven",
						"NominatedWon",
						"ShowName",
						"WorkID",
						"PersonName"
					},
					CreateTableCommand = "CREATE TABLE Honor(AwardID VARCHAR(200) PRIMARY KEY, AwardName VARCHAR(1000) NULL, YearGiven INT NULL, NominatedWon VARCHAR(50) NULL, ShowName VARCHAR(50) NULL, WorkID VARCHAR(50) NULL, PersonName VARCHAR(100) NULL);"
				}
			}, {
				"Television",
				new SQLTable () {
					Name = "Television", 
					ColumnNames = new string[] {
						"WorkID",
						"Title", 
						"Episodes",
						"Seasons",
						"StillRunning",
						"Network",
						"CameraSetup",
						"MinimumRuntime",
						"MaximumRuntime"
					},
					CreateTableCommand = "CREATE TABLE Television(WorkID VARCHAR(200) PRIMARY KEY, Title VARCHAR(400) NULL, Episodes INT NULL, Seasons INT NULL, StillRunning VARCHAR(3), Network VARCHAR(100) NULL, CameraSetup VARCHAR(200) NULL, MinimumRuntime INT NULL, MaximumRuntime INT NULL);"
				}
			}, {
				"Movies",
				new SQLTable () {
					Name = "Movies",
					ColumnNames = new string[] {
						"WorkID",
						"Title",
						"Rating",
						"BoxOffice",
						"Budget",
						"Year"
					},
					CreateTableCommand = "CREATE TABLE Movies(WorkID VARCHAR(200) PRIMARY KEY, Title VARCHAR(500) NULL, Rating VARCHAR(10) NULL, BoxOffice REAL NULL, Budget REAL NULL, Year INT NULL);"
				}
			}, {
				"Locations",
				new SQLTable () {
					Name = "Locations",
					ColumnNames = new string[] {
						"WorkID",
						"Location",
						"FilmedOrFiction",
						"Latitude",
						"Longitude"
					},
					CreateTableCommand = "CREATE TABLE Locations(WorkID VARCHAR(200), Location VARCHAR(500), FilmedOrFiction VARCHAR(50), Latitude REAL NULL, Longitude REAL NULL);"
				}
			}, {
				"GenreOf",
				new SQLTable () {
					Name = "GenreOf",
					ColumnNames = new string[] {
						"WorkID",
						"GenreName"
					},
					CreateTableCommand = "CREATE TABLE GenreOf(WorkID VARCHAR(200), GenreName VARCHAR(500));"
				}
			}, {
				"Works",
				new SQLTable () {
					Name = "Works",
					ColumnNames = new string[] {
						"WorkID",
						"TitleName"
					},
					CreateTableCommand = "CREATE TABLE Works(WorkID VARCHAR(200), TitleName VARCHAR(500));"
				}
			}
		};

		public SQLStructures (string filePath, string tableName)
		{
			FilePath = filePath;	
			TableName = tableName;
		}

		public DataTable GetData ()
		{
			DataTable table = new DataTable ();

			for (int i = 0; i < tableColumnDictionary [TableName].ColumnNames.Length; i++) {
				table.Columns.Add (new DataColumn (tableColumnDictionary [TableName].ColumnNames [i]));
			}

			using (StreamReader reader = new StreamReader (FilePath)) {
				string line = "";
				int c = 0;
				while ((line = reader.ReadLine ()) != null) {
					if (c > 0) {
						DataRow	row = table.NewRow ();
						string[] values = line.Split (new char[] { (char)9 });
						for (int i = 0; i < values.Length; i++) {
							row [i] = values [i];
						}
						table.Rows.Add (row);
					}
					c++;
				}
			}

			return table;
		}

		public string GetTableInsertCommand (DataRow row)
		{
			string[] columns = tableColumnDictionary [TableName].ColumnNames;

			string command = "INSERT INTO " + TableName + " (";
			for (int column = 0; column < columns.Length; column++) {
				if (column == columns.Length - 1) {
					command += columns [column] + ") ";
				} else {
					command += columns [column] + ", ";
				}
			}

			command += "VALUES (";
			for (int value = 0; value < row.ItemArray.Length; value++) {
				try {
					double columnValue = Convert.ToDouble (row.ItemArray [value].ToString ());
					command += columnValue;
				} catch (Exception) {
					string columnValue = row.ItemArray [value].ToString ();
					DateTime outDate;
					if (columnValue.Equals ("NULL") || columnValue.Length == 0)
						command += "NULL";
					else if (DateTime.TryParse (columnValue, out outDate)) {
						command += "STR_TO_DATE('" + columnValue + "', '%m/%d/%Y')";
					} else {
						if (columnValue.Contains ("\'"))
							columnValue = columnValue.Replace ("'", "''");
						command += "'" + columnValue + "'";
					}
				}

				if (value == row.ItemArray.Length - 1) {
					command += ");";
				} else {
					command += ", ";
				}
			}

			return command;
		}
	}
}

