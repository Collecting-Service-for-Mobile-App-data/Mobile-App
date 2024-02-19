namespace CordelUTE;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void GoToLoginClicked(object sender, EventArgs e)
	{
		// Use absolute routing for Shell navigation
		await Shell.Current.GoToAsync("//LoginPage");
	}

}


