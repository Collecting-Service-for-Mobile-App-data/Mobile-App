using System.Net;
using System.Text;
using System.Text.Json;
using CordelUTE;
using System.Net.Http.Headers;

namespace MauiApp1;

// Class responsible for interacting with the application's backend API.
public class ApiService
{
    private readonly HttpClient _httpClient; // HttpClient instance for making HTTP requests.
    private readonly string _baseUrl; // Base URL of the backend API.

    public ApiService()
    {
        _httpClient = new HttpClient();
        _baseUrl = "http://localhost:8080";
    }

    // Attempts to log in a user and retrieves the JWT token upon successful login.
    public async Task<(bool isLogIn, string errorMessage)> LoginAsync(LoginRequest request)
    {
        // Serialize the LoginRequest object to JSON format.
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        // Send a POST request to the API endpoint for login.
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/user/authenticate", content);
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string.
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Deserialize the JSON response into a dictionary.
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
            // Extract the JWT token from the response.
            var token = parsedJson["jwt"];
            // Store the JWT token in secure storage.
            await SecureStorage.Default.SetAsync("jwt", token);
            return (true, null);
        }
        else
        {
            // Read the error message from the response.
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }
    }

    // Retrieves the user ID associated with the stored JWT token.
    public async Task StoreUserId()
    {
        // Retrieve the JWT token from secure storage.
        var jwtToken = await SecureStorage.Default.GetAsync("jwt");

        // Set the Authorization header with the Bearer token.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        // Send a GET request to the API endpoint for retrieving user information.
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/user/sessionuser");

        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string.
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a dictionary.
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (parsedJson.ContainsKey("id"))
            {
                // Extract the user ID from the response.
                var userId = parsedJson["id"].ToString();

                // Store the user ID in secure storage.
                await SecureStorage.Default.SetAsync("userId", userId);
            }
        }
    }

    // Uploads a file to the backend API.
    public async Task<bool> UploadFileAsync(string filePath)
    {
        // Retrieve the JWT token from secure storage.
        var jwtToken = await SecureStorage.Default.GetAsync("jwt");

        // Set the Authorization header with the Bearer token.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        // Get the user information (assuming `getUser` is implemented as shown).
        var user = await getUser();

        // Create a multipart form data content for the file upload.
        var multiContent = new MultipartFormDataContent();

        // Set the date to the current Unix timestamp in milliseconds.
        var date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        // Read the file bytes.
        var fileBytes = await File.ReadAllBytesAsync(filePath);

        // Create a ByteArrayContent for the file data.
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        // Add data to the multipart content.
        multiContent.Add(new StringContent(date), "date");
        multiContent.Add(new StringContent(user.id.ToString()), "userId");
        multiContent.Add(new StringContent(user.companyId.ToString()), "companyId");
        multiContent.Add(new StringContent("false"), "isChecked"); // Might need to be adjusted based on your API requirements.
        multiContent.Add(fileContent, "file", Path.GetFileName(filePath));

        // Send a POST request to the API endpoint for file upload.
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

    // Retrieves user information based on the stored user ID.
    private async Task<UserRequest> getUser()
    {
        // Retrieve the JWT token from secure storage.
        var jwtToken = await SecureStorage.Default.GetAsync("jwt");

        // Set the Authorization header with the Bearer token.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        // Retrieve the user ID from secure storage.
        var id = await SecureStorage.Default.GetAsync("userId");

        // Send a GET request to the API endpoint for retrieving user information.
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/user/getuser/{id}");

        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string.
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response using JsonDocument.
            var jsonObject = JsonDocument.Parse(jsonResponse).RootElement;

            // Create a new User object and populate its properties from the JSON data.
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

    // Attempts to register a new user with the provided signup request.
    public async Task<bool> SignupAsync(SignupRequest request)
    {
        try
        {
            // Serialize the SignupRequest object to JSON format.
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send a POST request to the API endpoint for signup.
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/user", content);

            // Return true if the signup request was successful (status code 200).
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log or handle the exception appropriately.
            Console.WriteLine($"An exception occurred during signup: {ex.Message}");
            return false;
        }
    }

    // Retrieves a list of companies from the backend API.
    public async Task<List<CompanyRequest>> GetCompaniesAsync()
    {
        var companies = new List<CompanyRequest>();
        try
        {
            // Send a GET request to the API endpoint for retrieving companies.
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/company/companies");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Read the response content as a string.
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a list of Company objects.
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
