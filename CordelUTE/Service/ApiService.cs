using System.Net;
using System.Text;
using System.Text.Json;
using CordelUTE;
using System.Net.Http.Headers;

namespace MauiApp1;

/// <summary>
/// Class responsible for interacting with the application's backend API.
/// </summary>
public class ApiService
{
    private readonly HttpClient _httpClient; // HttpClient instance for making HTTP requests.
    private readonly string _baseUrl; // Base URL of the backend API.

    public ApiService()
    {
        _httpClient = new HttpClient();
        _baseUrl = "http://localhost:8080";
    }

    /// <summary>
    /// Sends a login request to the API endpoint and returns the result.
    /// </summary>
    /// <param name="request">The login request object.</param>
    /// <returns>A tuple containing a boolean indicating if the login was successful and an error message if applicable.</returns>
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
            //await SecureStorage.Default.SetAsync("jwt", token);
            return (true, null);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }
    }

    /// <summary>
    /// Retrieves the user ID associated with the stored JWT token.
    /// </summary>
    public async Task StoreUserId()
    {
        var jwtToken = await SecureStorage.Default.GetAsync("jwt");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/user/sessionuser");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (parsedJson.ContainsKey("id"))
            {
                var userId = parsedJson["id"].ToString();
                await SecureStorage.Default.SetAsync("userId", userId);
            }
        }
    }

    /// <summary>
    /// Uploads a file to the backend API.
    /// </summary>
    /// <param name="filePath">The path of the file to be uploaded.</param>
    /// <returns>A boolean indicating if the file upload was successful.</returns>
    public async Task<bool> UploadFileAsync(string filePath)
    {
        var jwtToken = await SecureStorage.Default.GetAsync("jwt");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var user = await getUser();

        var multiContent = new MultipartFormDataContent();
        var date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        multiContent.Add(new StringContent(date), "date");
        multiContent.Add(new StringContent(user.id.ToString()), "userId");
        multiContent.Add(new StringContent(user.companyId.ToString()), "companyId");
        multiContent.Add(new StringContent("false"), "isChecked");
        multiContent.Add(fileContent, "file", Path.GetFileName(filePath));

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/sqlite-files/upload", multiContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("File uploaded successfully.");
            return true;
        }
        else
        {
            Console.WriteLine($"File upload failed with status: {response.StatusCode}");
            return false;
        }
    }

    /// <summary>
    /// Retrieves user information based on the stored user ID.
    /// </summary>
    /// <returns>A UserRequest object containing the user information.</returns>
    private async Task<UserRequest> getUser()
    {
        var jwtToken = await SecureStorage.Default.GetAsync("jwt");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var id = await SecureStorage.Default.GetAsync("userId");
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/user/getuser/{id}");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(jsonResponse).RootElement;
            var user = new UserRequest
            {
                id = jsonObject.GetProperty("id").GetInt64(),
                email = jsonObject.GetProperty("email").GetString(),
                companyId = jsonObject.GetProperty("company").GetProperty("id").GetInt64()
            };

            return user;
        }

        return null;
    }

    /// <summary>
    /// Attempts to register a new user with the provided signup request.
    /// </summary>
    /// <param name="request">The signup request object.</param>
    /// <returns>A boolean indicating if the signup was successful.</returns>
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
            Console.WriteLine($"An exception occurred during signup: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Retrieves a list of companies from the backend API.
    /// </summary>
    /// <returns>A list of CompanyRequest objects representing the companies.</returns>
    public async Task<List<CompanyRequest>> GetCompaniesAsync()
    {
        var companies = new List<CompanyRequest>();
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/company/companies");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                companies = JsonSerializer.Deserialize<List<CompanyRequest>>(responseContent);
            }
            else
            {
                Console.WriteLine($"Failed to retrieve companies: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving companies: {e}");
        }

        return companies;
    }
}
