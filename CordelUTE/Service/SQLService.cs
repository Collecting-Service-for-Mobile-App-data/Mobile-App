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
			using (var connection = OpenConnection($"Data Source=C:\\Users\\Ole Kristian\\Desktop\\database.db"))
			{
				SetWalMode(connection);
				CopyDatabase();
				CloseWalMode(connection);
				connection.Close();
			}
		}

		private void CopyDatabase() {
			try {
				string sourceFile = "C:\\Users\\Ole Kristian\\Desktop\\database.db";
				string destinationPath = "../Mobile-App\\CordelUTE\\DatabaseTempFiles/database.db";
				string destinationFile = Path.GetFullPath(destinationPath);
				File.Copy(sourceFile, destinationFile, true);
			}
			catch(IOException ioEx) {
				Console.WriteLine("Error copying database: " + ioEx.Message);
			}
		}

        // Open the database connection
		private SqliteConnection OpenConnection(string connectionString)
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
				// Set the journal_mode to WAL
				command.CommandText = "PRAGMA journal_mode=WAL;";
				var result = command.ExecuteScalar().ToString();

				// Optionally, you can check if the setting was successful
				if (!result.Equals("wal", StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException("Failed to set journal_mode to WAL.");
				}
			}
		}

		private void CloseWalMode(SqliteConnection connection) {
			using (var checkpointCommand = new SqliteCommand("PRAGMA wal_checkpoint(FULL);", connection))
            {
                checkpointCommand.ExecuteNonQuery();
            }
			using (var command = connection.CreateCommand()) {
				command.CommandText ="PRAGMA journal_mode=DELET;";
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
