using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace MauiApp1
{
    public class SQLService
    {
        public static string DbFileName = "SampleDatabase.db";
        public static string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbFileName);

        // Default constructor
        public SQLService() {}

        // Open and configure the database
        public void ConfigureDatabase()
        {
            using (var connection = OpenConnection($"Data Source={DbPath}"))
            {
                SetWalMode(connection);
            }
        }

        // Open the database connection
		public SqliteConnection OpenConnection(string connectionString)
		{
			var connection = new SqliteConnection(connectionString);
			try
			{
				connection.Open();
				if (connection.State == System.Data.ConnectionState.Open)
				{
					Console.WriteLine("Database is open.");
				}
				else
				{
					Console.WriteLine("Database is not open.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to open database: {ex.Message}");
			}
			return connection;
		}


        // Set Write-Ahead Logging mode
        private void SetWalMode(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA journal_mode=WAL;";
                command.ExecuteNonQuery();
            }
        }

        // Get the path to the database based on the platform
        public string GetPath()
        {
            var currentPlatform = DeviceInfo.Platform;
            string sqlitedbpath = "";
            if (currentPlatform == DevicePlatform.iOS)
            {
                sqlitedbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DbFileName);
            }
            else if (currentPlatform == DevicePlatform.Android)
            {
                sqlitedbpath = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
            }
            else
            {
                sqlitedbpath = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
            }
            Console.WriteLine(sqlitedbpath);
            return sqlitedbpath;
        }
    }
}
