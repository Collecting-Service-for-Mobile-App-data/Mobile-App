using MauiApp1;
using Microsoft.Maui.Controls;
using System;

namespace CordelUTE

{
    public partial class SignupPage : ContentPage
    {
        private ApiService _apiService = new ApiService();
        public SignupPage()
        {
            InitializeComponent();
        }

        private async void OnSignupButtonClicked(object sender, EventArgs e)
        {
            // Simple validation
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
                PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                await DisplayAlert("Validation Failed", "Please check your inputs and try again.", "OK");
                return;
            }

            var signupRequest = new SignupRequest
            {
                Company = CompanyEntry.Text,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text,
                // Add any other required fields
            };
            
            // Assuming _apiService.SignupAsync(signupRequest) exists and works similarly to LoginAsync
            var isSuccess = await _apiService.SignupAsync(signupRequest);
            if (isSuccess)
            {
                await DisplayAlert("Success", "Signup successful.", "OK");
                // Optionally navigate to the login page or main app page
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await DisplayAlert("Error", "Signup failed. Please try again.", "OK");
            }
        }


        // Make sure the method name matches the one specified in the XAML
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page, using the correct route name as defined in your AppShell.xaml
            await Shell.Current.GoToAsync("//LoginPage"); // ".." navigates one level up in the navigation stack
        }
    }
}
