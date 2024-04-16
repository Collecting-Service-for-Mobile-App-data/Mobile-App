using System.Data.SQlite;
using Microsoft.Maui.Devices;
namespace MauiApp1;


public class SQLService
{
	public SQLService()
	{
		
	}	

	private bool DeviceCheck() {
		if (Device.RuntimePlatform == DevicePlatform.iOS){
			return true;
		}
		else if (Device.RuntimePlatform == DevicePlatform.Android){
			return false;
		}
	}
}