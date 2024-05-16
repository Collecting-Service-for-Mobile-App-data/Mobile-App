using MauiApp1;

namespace CordelUTE
{
    /// <summary>
    /// Represents the login page of the application.
    /// </summary>
    public partial class LoginPage : ContentPage
    {
        private ApiService _apiService = new ApiService();

        /// <summary>
        /// Default constructor for the LoginPage class. This constructor
        /// is called automatically when the LoginPage component is created.
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the login button click event. Sends a login request to the API
        /// and navigates to the main page if successful.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
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
                ErrorMessageLabel.IsVisible = false;
                await Shell.Current.GoToAsync("//MainPage");
                await _apiService.StoreUserId();
                Console.WriteLine(SecureStorage.Default.GetAsync("useId").ToString());
            }
            else
            {
                ErrorMessageLabel.Text = errorMessage;
                ErrorMessageLabel.IsVisible = true;
            }
        }

        /// <summary>
        /// Handles the back button click event. Navigates back to the main page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        /// <summary>
        /// Handles the signup button click event. Navigates to the signup page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void SignupClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//SignupPage");
        }
    }
}
