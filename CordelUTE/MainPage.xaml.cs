namespace CordelUTE;
using Microsoft.Maui.Controls;
using MauiApp1;
using System;

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

	public async void UploadFile(object sender, EventArgs e) {
		SQLService sQLService = new SQLService();
		ApiService apiService = new ApiService();
        await sQLService.ConfigureDatabase();
        await apiService.UploadFileAsync(sQLService.GetPathToCopyDatabase() + "\\database.db");
	}
}