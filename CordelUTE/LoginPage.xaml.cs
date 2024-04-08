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

    // Call LoginAsync and receive both token and error message
    var (token, errorMessage) = await _apiService.LoginAsync(loginRequest);

    if (!string.IsNullOrEmpty(token))
    {
        // Handle success
        await SecureStorage.SetAsync("jwtToken", token);
        await Shell.Current.GoToAsync("//MainPage");
    }
    else
    {
        // Handle failure and print the error message
        Console.WriteLine(errorMessage); // Print the error message to the console
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
