namespace CordelUTE
{
    using Microsoft.Maui.Controls;
    using MauiApp1;
    using System;

    /// <summary>
    /// Represents the main page of the application.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Default constructor for the MainPage class. This constructor
        /// is called automatically when the MainPage component is created.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the event when the "Go to Login" button is clicked. Navigates to the login page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void GoToLoginClicked(object sender, EventArgs e)
        {
            // Use absolute routing for Shell navigation
            await Shell.Current.GoToAsync("//LoginPage");
        }

        /// <summary>
        /// Handles the event when the upload file button is clicked. Configures the database and uploads the file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public async void UploadFile(object sender, EventArgs e)
        {
            SQLService sQLService = new SQLService();
            ApiService apiService = new ApiService();
            await sQLService.ConfigureDatabase();
            await apiService.UploadFileAsync(sQLService.GetPathToCopyDatabase() + "\\database.db");
            DeleteFile();
            await ShowErrorMessage();
        }

        /// <summary>
        /// Shows an error message using a display alert.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task ShowErrorMessage()
        {
            await DisplayAlert("Error", "Error Sent", "OK");
        }

        /// <summary>
		/// Deletes a file.
		/// </summary>
		private void DeleteFile()
		{
			string destinationPath = "../Mobile-App\\CordelUTE\\DatabaseTempFiles/database.db";
			string destinationFile = Path.GetFullPath(destinationPath);
			File.Delete(destinationFile);
		}
    }
}
