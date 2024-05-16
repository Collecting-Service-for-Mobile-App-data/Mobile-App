using MauiApp1;

namespace CordelUTE
{
    /// <summary>
    /// Represents the signup page of the application.
    /// </summary>
    public partial class SignupPage : ContentPage
    {
        private ApiService _apiService = new ApiService();
        private List<CompanyRequest> companies = new List<CompanyRequest>();
        private CompanyRequest SelectedCompany;

        /// <summary>
        /// Default constructor for the SignupPage class. This constructor
        /// is called automatically when the SignupPage component is created.
        /// </summary>
        public SignupPage()
        {
            InitializeComponent();
            InitializeCompany();
        }

        /// <summary>
        /// Initializes the list of companies by fetching them from the API.
        /// </summary>
        private async void InitializeCompany()
        {
            companies = await _apiService.GetCompaniesAsync();
        }

        /// <summary>
        /// Handles the signup button click event. Validates input and sends a signup request to the API.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnSignupButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
                PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                SignupErrorMessageLabel.Text = "Please check your inputs and try again.";
                SignupErrorMessageLabel.IsVisible = true;
                return;
            }

            var signupRequest = new SignupRequest
            {
                company = SelectedCompany,
                email = EmailEntry.Text,
                password = PasswordEntry.Text,
            };
            var (isSuccess, errorMessage) = await _apiService.SignupAsync(signupRequest);
            if (isSuccess)
            {
                SignupErrorMessageLabel.IsVisible = false;
                await DisplayAlert("Success", "Signup successful.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                SignupErrorMessageLabel.Text = errorMessage;
                SignupErrorMessageLabel.IsVisible = true;
            }
        }

        /// <summary>
        /// Handles the back button click event. Navigates back to the login page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        /// <summary>
        /// Handles the text changed event for the company entry. Filters the company list based on the input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CompanyEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue.ToLower();
            var filteredCompanies = companies.Where(c => c.name.ToLower().Contains(searchText)).ToList();
            CompanyListView.ItemsSource = filteredCompanies;
            CompanyListView.IsVisible = filteredCompanies.Any();
        }

        /// <summary>
        /// Handles the item selected event for the company list view. Sets the selected company and updates the company entry text.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CompanyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is CompanyRequest selectedCompany)
            {
                CompanyEntry.Text = selectedCompany.name;
                SelectedCompany = selectedCompany;
                CompanyListView.IsVisible = false;
            }
        }
    }
}
