using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MauiApp1.Tests")]

namespace MauiApp1
{
	/// <summary>
	/// Class responsible for database operations.
	/// </summary>
	public class SQLService
	{
		public static string dbFileName = "database.db";

		/// <summary>
		/// Default constructor for the SQLService class.
		/// </summary>
		public SQLService() { }

		/// <summary>
		/// Opens and configures the database.
		/// </summary>
		public async Task ConfigureDatabase()
		{
			string mainDatabase = $"Data Source={GetPathToDatabase()}\\database.db;Pooling=False";
			var connection = OpenConnection(mainDatabase);
			{
				try
				{
					await SetWalMode(connection);
					CloseConnection(connection);
					await TestEdditDatabase();
					await CopyDatabase();
					connection = OpenConnection(mainDatabase);
					await CloseWalModeAndSaveChanges(connection);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
				finally
				{
					CloseConnection(connection);
				}
			}
			await ConfigCopyDatabase();
		}

		/// <summary>
		/// Configures the copy data to not be in WAL mode.
		/// </summary>
		private async Task ConfigCopyDatabase()
		{
			string copyDatabase = $"Data Source={GetPathToCopyDatabase()}\\database.db;Pooling=False";
			var connection = OpenConnection(copyDatabase);
			{
				try
				{
					await ColseWalModeAndNotSaveChanges(connection);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
				finally
				{
					CloseConnection(connection);
				}
			}
		}

		/// <summary>
		/// Copies the database file.
		/// </summary>
		internal async Task CopyDatabase()
		{
			try
			{
				string sourceFile = GetPathToDatabase() + "\\database.db";
				string destinationFile = GetPathToCopyDatabase() + "\\database.db";
				File.Copy(sourceFile, destinationFile, true);
			}
			catch (IOException ioEx)
			{
				Console.WriteLine("Error copying database: " + ioEx.Message);
			}
		}

		/// <summary>
		/// Opens the database connection.
		/// </summary>
		/// <param name="connectionString">The connection string for the database.</param>
		/// <returns>The opened SqliteConnection object.</returns>
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

		/// <summary>
		/// Sets Write-Ahead Logging mode.
		/// </summary>
		/// <param name="connection">The SqliteConnection object.</param>
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

		/// <summary>
		/// Merges the WAL file and main database file and closes WAL mode.
		/// </summary>
		/// <param name="connection">The SqliteConnection object.</param>
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

		/// <summary>
		/// Closes WAL mode without merging the WAL file and main database file.
		/// </summary>
		/// <param name="connection">The SqliteConnection object.</param>
		private async Task ColseWalModeAndNotSaveChanges(SqliteConnection connection)
		{
			await Task.Run(() =>
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "PRAGMA journal_mode=DELETE;";
					command.ExecuteNonQuery();
				}
			});
		}

		/// <summary>
		/// Closes the connection to a database.
		/// </summary>
		/// <param name="connection">The SqliteConnection object.</param>
		private void CloseConnection(SqliteConnection connection)
		{
			connection.Close();
			if (connection.State == System.Data.ConnectionState.Closed)
			{
				Console.WriteLine("Close");
			}
		}

		/// <summary>
		/// Test method to add a table to a database.
		/// </summary>
		private async Task TestEdditDatabase()
		{
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
				finally
				{
					CloseConnection(connection);
				}
			}
		}

		/// <summary>
		/// Returns the path to the main database file.
		/// </summary>
		/// <returns>The path to the main database file.</returns>
		internal string GetPathToDatabase()
		{
			return Directory.GetCurrentDirectory() + "\\CordelUTE\\Database";
		}

		/// <summary>
		/// Returns the path to the copy database file.
		/// </summary>
		/// <returns>The path to the copy database file.</returns>
		public string GetPathToCopyDatabase()
		{
			return Directory.GetCurrentDirectory() + "\\CordelUTE\\DatabaseTempFiles";
		}

		/// <summary>
		/// Gets the path to the database based on the platform.
		/// </summary>
		/// <returns>The path to the database file.</returns>
		public string GetPath()
		{
			var currentPlatform = DeviceInfo.Platform;
			string sqlitedbpath = "";
			if (currentPlatform == DevicePlatform.iOS)
			{
				sqlitedbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", dbFileName);
			}
			else if (currentPlatform == DevicePlatform.Android)
			{
				sqlitedbpath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);
			}
			else
			{
				string rootPath = @"C:\";
				string searchPattern = dbFileName;

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

		/// <summary>
		/// Recursively searches for files in a directory and its subdirectories.
		/// </summary>
		/// <param name="path">The starting directory.</param>
		/// <param name="searchPattern">The search pattern for file names.</param>
		/// <param name="foundFiles">The list to store found file paths.</param>
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
