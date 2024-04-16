using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CordelUTE;
using System.Net;
using System.Net.Http;

namespace MauiApp1;


public class SQLService
{
	public SQLService()
	{

	}

	[DllImport("sqlite3", EntryPoint = "sqlite3_snapshot_get", CallingConvention = CallingConvention.Cdecl)]
    public static extern int sqlite3_snapshot_get(IntPtr db, string zSchema, out IntPtr ppSnapshot);

	[DllImport("sqlite3", EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl)]
	public static extern int sqlite3_open(char filename, sqlite3 ppDb)


	public async Task getSQLiteFile() {
		var currentPlatform = DeviceInfo.Platform;
		if(currentPlatform == DevicePlatform.iOS) {
			sqlite3_snapshot_get();

		}
		else if(currentPlatform == DevicePlatform.Android) {
			sqlite3_snapshot_get();
		}
	}


}