using Microsoft.Maui.Controls;
using System;

namespace CordelUTE
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }


        // Make sure the method name matches the one specified in the XAML
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page, using the correct route name as defined in your AppShell.xaml
            await Shell.Current.GoToAsync("//LoginPage"); // ".." navigates one level up in the navigation stack
        }
    }
}
