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
            var (isLogIn, errorMessage) = await _apiService.LoginAsync(loginRequest);
            if (isLogIn)
            {
                await Shell.Current.GoToAsync("//MainPage");
                await _apiService.StoreUserId();
                Console.WriteLine(SecureStorage.Default.GetAsync("useId").ToString());
            }
            else
            {
                Console.WriteLine(errorMessage);
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
