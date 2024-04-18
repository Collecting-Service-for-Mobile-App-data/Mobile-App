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

        private void OnPrintDeviceInfoClicked(object sender, EventArgs e)
        {
            SQLService sQLService = new SQLService();

            // Print device information to the console
            //Console.WriteLine($"Device Model: {deviceModel}");
            //Console.WriteLine($"Device Manufacturer: {deviceManufacturer}");
            //Console.WriteLine($"Device Name: {deviceName}");
            sQLService.InitializeDatabase();
            sQLService.GetPath();
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
