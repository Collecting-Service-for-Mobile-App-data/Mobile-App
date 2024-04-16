using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel;


namespace MauiApp1;


public class SQLService
{
	public SQLService()
	{

	}

	public bool DeviceCheck()
	{
		// Use DeviceInfo to get the current platform
		var currentPlatform = DeviceInfo.Platform;

		if (currentPlatform == DevicePlatform.iOS)
		{
			return true; // Return true for iOS
		}
		else if (currentPlatform == DevicePlatform.Android)
		{
			return false; // Return false for Android
		}
		else
		{
			// Optionally handle other platforms or default case
			return false;
		}
	}
}