using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    public class SQLService
    {
        public static string dbFileName = "database.db";

        // Default constructor
        public SQLService() {}

        // Open and configure the database
		public async Task ConfigureDatabase()
		{
			string mainDatabase = $"Data Source={GetPathToDatabase()}\\database.db;Pooling=False";
			var connection = OpenConnection(mainDatabase);
			{
				try {
					await SetWalMode(connection);
					CloseConnection(connection);
					await TestEdditDatabase();
					await CopyDatabase();
					connection = OpenConnection(mainDatabase);
					await CloseWalModeAndSaveChanges(connection);
				}
				catch(Exception e){
					Console.WriteLine(e);
				}
				finally {
					CloseConnection(connection);
				}
			}
			await ConfigCopyDatabase();
		}

		//Config the copy data to not be inn wal mode
		private async Task ConfigCopyDatabase() {
			string copyDatabase = $"Data Source={GetPathToCopyDatabase()}\\database.db;Pooling=False";
			var connection = OpenConnection(copyDatabase);
			{
				try {
					await ColseWalModeAndNotSaveChanges(connection);
				}
				catch(Exception e) {
					Console.WriteLine(e);
				}
				finally {
					CloseConnection(connection);
				}
			}

		}

		//Copy a database file
		private async Task CopyDatabase() {
			try {
				string sourceFile = GetPathToDatabase() + "\\database.db";
				string destinationFile = GetPathToCopyDatabase() + "\\database.db";
				File.Copy(sourceFile, destinationFile, true);
			}
			catch(IOException ioEx) {
				Console.WriteLine("Error copying database: " + ioEx.Message);
			}
		}

        // Open the database connection
		private SqliteConnection OpenConnection(string connectionString)
		{
			Console.WriteLine(connectionString);
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
		private async Task SetWalMode(SqliteConnection connection)
		{
			await Task.Run(() =>
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "PRAGMA journal_mode=WAL;";
					var result = command.ExecuteScalar().ToString();
					if (!result.Equals("wal", StringComparison.OrdinalIgnoreCase))
					{
						throw new InvalidOperationException("Failed to set journal_mode to WAL.");
					}
				}
			});
		}
		
		//Merge the wal file and main database file.
		//Closes wal mode
		private async Task CloseWalModeAndSaveChanges(SqliteConnection connection)
		{
			await Task.Run(() =>
			{
				using (var checkpointCommand = new SqliteCommand("PRAGMA wal_checkpoint(FULL);", connection))
				{
					checkpointCommand.ExecuteNonQuery();
				}
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "PRAGMA journal_mode=DELETE;";
					command.ExecuteNonQuery();
				}
			});
		}

		//Closes wal mode without merging the wal file and main database file.
		private async Task ColseWalModeAndNotSaveChanges(SqliteConnection connection) {
			await Task.Run(() => {
				using (var command = connection.CreateCommand()) {
					command.CommandText = "PRAGMA journal_mode=DELETE;";
					command.ExecuteNonQuery();
				}
			});
		}

		//Closes connection to a database
		private void CloseConnection(SqliteConnection connection) {
			connection.Close();
			if(connection.State == System.Data.ConnectionState.Closed) {
				Console.WriteLine("Close");
			}
		}

		//Test method to add a table to a database
		private async Task TestEdditDatabase() {
			string mainDatabase = $"Data Source={GetPathToDatabase()}\\database.db;Pooling=False";
			using (var connection = OpenConnection(mainDatabase))
    		{
				var command = connection.CreateCommand();
				command.CommandText = @"CREATE TABLE IF NOT EXISTS MyTable (
										Id INTEGER PRIMARY KEY,
										Name TEXT NOT NULL,
										Email TEXT NOT NULL
									);";

				try
				{
					command.ExecuteNonQuery();
					Console.WriteLine("Table 'MyTable' checked/created successfully.");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to create/check table 'MyTable': {ex.Message}");
				}
				finally {
					CloseConnection(connection);
				}
			}
		}

		//Delete a file
		private void DelteFile() {

			string destinationPath = "../Mobile-App\\CordelUTE\\DatabaseTempFiles/database.db";
			string destinationFile = Path.GetFullPath(destinationPath);
			File.Delete(destinationFile);
		}

		//Return the path to the main database file
		private string GetPathToDatabase() {
			return Directory.GetCurrentDirectory() + "\\CordelUTE\\Database";
		}

		//Return the path to the copy database file
		public string GetPathToCopyDatabase() {
			return Directory.GetCurrentDirectory() + "\\CordelUTE\\DatabaseTempFiles";
		}








        // Get the path to the database based on the platform
		// 
        public string GetPath()
        {
            var currentPlatform = DeviceInfo.Platform;
            string sqlitedbpath = "";
            if (currentPlatform == DevicePlatform.iOS)
            {
                //sqlitedbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", dbFileName);
            }
            else if (currentPlatform == DevicePlatform.Android)
            {
                //sqlitedbpath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);
            }
            else
            {
				string rootPath = @"C:\"; // Starting directory
				string searchPattern = dbFileName; // The name of the file to search for

				List<string> foundFiles = new List<string>();
				SearchFiles(rootPath, searchPattern, foundFiles);

				Console.WriteLine("Found files:");
				foreach (string file in foundFiles)
				{
					Console.WriteLine(file);
				}
            }
            Console.WriteLine(sqlitedbpath);
            return sqlitedbpath;
        }
			 private void SearchFiles(string path, string searchPattern, List<string> foundFiles)
    {
        try
        {
            // Get all files in the current directory
            string[] files = Directory.GetFiles(path, searchPattern);
            foundFiles.AddRange(files);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"No access to {path}, skipping...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred accessing {path}: {ex.Message}");
        }

        try
        {
            // Get all subdirectories in the current directory
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                SearchFiles(directory, searchPattern, foundFiles);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"No access to subdirectories in {path}, skipping...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred accessing subdirectories in {path}: {ex.Message}");
        }
    }
		
    }
}
