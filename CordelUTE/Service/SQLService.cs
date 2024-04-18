using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CordelUTE;
using System.Net;
using System.Net.Http;
using Microsoft.Data.Sqlite;

namespace MauiApp1;


public class SQLService
{

	public delegate int CallbackDelegate(IntPtr userData, int numColumns, IntPtr columnValues, IntPtr columnNames);
	public static string DbFileName = "SampleDatabase.db";
    public static string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbFileName);

	[DllImport("sqlite3", EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl)]
	public static extern int sqlite3_open(string filename, out IntPtr ppDb);

	[DllImport("sqlite3", EntryPoint = "sqlite3_exec", CallingConvention = CallingConvention.Cdecl)]
	public static extern int sqlite3_exec(IntPtr db, string sql, CallbackDelegate callback, IntPtr userData, out IntPtr errMsg);

	//Default constructor
	public SQLService(){}

	//Get the database as a file
	public void GetSQLiteFile() {
		IntPtr database = OpenDatabase(GetPath());
		SetWalMode(database);
		
	}

	//Get the path to the database
	public string GetPath() {
		var currentPlatform = DeviceInfo.Platform;
		string sqlitedbpath = "";
		string databaseName = "main.db";
		if(currentPlatform == DevicePlatform.iOS) {
			sqlitedbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName);

		}
		else if(currentPlatform == DevicePlatform.Android) {
			//sqlitedbpath = Path.Combine(FileSystem.AppDataDirectory, databaseName);
			sqlitedbpath = "android";
		}
		else {
			sqlitedbpath = Path.Combine(FileSystem.AppDataDirectory, databaseName);
		}
		Console.WriteLine($"database = {sqlitedbpath}");
		return sqlitedbpath;
	}

	//Open the database
	private IntPtr OpenDatabase(string pathToDatabase) {
		IntPtr database;
		sqlite3_open(pathToDatabase, out database);
		return database;
	}

	//Set the database in WAL mode
	private void SetWalMode(IntPtr database) {
		IntPtr errorMessage;
		sqlite3_exec(database, "PRIGMA journal_mode=WAL", null, IntPtr.Zero, out errorMessage);
	}

    public void InitializeDatabase()
    {
        using (var db = new SqliteConnection($"Filename={DbPath}"))
        {
            db.Open();

            string tableCommand = "CREATE TABLE IF NOT EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, email TEXT)";

            SqliteCommand createTable = new SqliteCommand(tableCommand, db);

            createTable.ExecuteReader();

            //SqliteCommand insertCommand = new SqliteCommand
            //{
                //Connection = db,
              //  CommandText = "INSERT INTO MyTable (primary_key, email) VALUES (1, 'sander@gmail.com');"
            //};
            //insertCommand.Parameters.AddWithValue("@Entry", "This is a test entry");

            //insertCommand.ExecuteReader();
        }
    }
}