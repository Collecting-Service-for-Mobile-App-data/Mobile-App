namespace CordelUTE;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	void OnButtonClicked(object sender, EventArgs args)
	{
		count++;
		((Button)sender).Text = $"You clicked {count} times.";
	}
}

