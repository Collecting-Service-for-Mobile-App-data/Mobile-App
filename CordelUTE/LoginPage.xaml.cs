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
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text
            };

            // Use the instance of ApiService (_apiService) to call LoginAsync
            var token = await _apiService.LoginAsync(loginRequest);
            if (!string.IsNullOrEmpty(token))
            {
                // Handle success. For example, save the token for later use and navigate to the main app page
                await SecureStorage.SetAsync("jwtToken", token); // Using SecureStorage for better security
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                // Handle failure. Show an error message to the user
                await DisplayAlert("Login Failed", "Please check your credentials and try again.", "OK");
            }
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
