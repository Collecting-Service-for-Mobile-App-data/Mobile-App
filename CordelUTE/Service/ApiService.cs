using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CordelUTE;

namespace MauiApp1;

public class ApiService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseUrl = "http://localhost:8080";

    public ApiService()
    {
        // If you have any initialization logic, it can go here.
        // For example, setting up headers that are common to all requests.
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/user/authenticate", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Assuming the response contains the JWT token directly or an object that can be deserialized to get the token.
            return jsonResponse;
        }
        else
        {
            // Optionally log or handle the error response here.
            return null;
        }
    }

    public async Task<bool> SignupAsync(SignupRequest request)
    {
        try
        {
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            // Adjust the URL if your signup endpoint is different.
            var response = await _httpClient.PostAsync($"{_baseUrl}/", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed.
            Console.WriteLine($"An exception occurred during signup: {ex.Message}");
            return false;
        }
    }
}
