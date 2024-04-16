using MauiApp1;
using Microsoft.Maui.Controls;

namespace CordelUTE
{
    public partial class LoginPage : ContentPage
    {
        private ApiService _apiService = new ApiService();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var loginRequest = new LoginRequest
            {
                email = EmailEntry.Text,
                password = PasswordEntry.Text
            };

            // Call LoginAsync and receive both token and error message
            var (token, errorMessage) = await _apiService.LoginAsync(loginRequest);

            if (!string.IsNullOrEmpty(token))
            {
                // Handle success
                await Shell.Current.GoToAsync("//MainPage");
                Console.WriteLine(token);
                await SecureStorage.SetAsync("jwtToken", token);
            }
            else
            {
                // Handle failure and print the error message
                Console.WriteLine(errorMessage); // Print the error message to the console
                await DisplayAlert("Login Failed", "Please check your credentials and try again.", "OK");
            }
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

        private void OnPrintDeviceInfoClicked(object sender, EventArgs e)
        {
            // Get device model and manufacturer
            string deviceModel = DeviceInfo.Model;
            string deviceManufacturer = DeviceInfo.Manufacturer;
            string deviceName = DeviceInfo.Name;  // Gets the device name set by the user.

            // Print device information to the console
            Console.WriteLine($"Device Model: {deviceModel}");
            Console.WriteLine($"Device Manufacturer: {deviceManufacturer}");
            Console.WriteLine($"Device Name: {deviceName}");
            Console.WriteLine(DeviceCheck());
        }



        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (usually the MainPage)
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void SignupClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (usually the MainPage)
            await Shell.Current.GoToAsync("//SignupPage");
        }
    }
}
