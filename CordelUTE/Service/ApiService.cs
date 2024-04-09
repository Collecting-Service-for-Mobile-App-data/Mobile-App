using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CordelUTE;

namespace MauiApp1;

public class ApiService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseUrl = "http://127.0.0.1:8080";

    public ApiService()
    {
        // If you have any initialization logic, it can go here.
        // For example, setting up headers that are common to all requests.
    }

    public async Task<(string token, string errorMessage)> LoginAsync(LoginRequest request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        Console.WriteLine(request);
        Console.WriteLine(jsonRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/user/authenticate", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Assuming the response contains the JWT token directly or an object that can be deserialized to get the token.
            return (jsonResponse, null); // Return the token and null for errorMessage when successful
        }
        else
        {
            // Read the response body for the error message
            var errorMessage = await response.Content.ReadAsStringAsync();
            // Return null for token and the actual error message when the request fails
            return (null, errorMessage);
        }
    }

    

    public async Task<bool> SignupAsync(SignupRequest request)
    {
        try
        {
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            // Adjust the URL if your signup endpoint is different.
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/user", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed.
            Console.WriteLine($"An exception occurred during signup: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Company>> GetCompaniesAsync() {
        List<Company> companies = new List<Company>();
        try {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/company/companies");
            if(response.StatusCode == HttpStatusCode.OK) {
                var responseContent = await response.Content.ReadAsStringAsync();
                companies = JsonSerializer.Deserialize<List<Company>>(responseContent);
            }
            else {
                Console.WriteLine(response.StatusCode.ToString());
            }
        }
        catch(Exception e) {
            Console.WriteLine(e);
        }
        return companies;
    }
}
