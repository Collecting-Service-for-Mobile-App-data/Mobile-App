	namespace MauiApp1;

	// Represents a user entity within the application.
public class User
{
    // The unique identifier for the user.
    public required long id { get; set; }

    // The email address of the user.
    public required string email { get; set; }

    // The unique identifier of the company the user belongs to.
    public required long companyId { get; set; }
}