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
    private readonly string _baseUrl = "http://localhost:8080";

    public ApiService()
    {

    }

    public async Task<(bool isLogIn, string errorMessage)> LoginAsync(LoginRequest request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/user/authenticate", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
            var token = parsedJson["jwt"];
            await SecureStorage.Default.SetAsync("jwt", token);
            return (true, null);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }
    }

    

    public async Task<bool> SignupAsync(SignupRequest request)
    {
        try
        {
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
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
