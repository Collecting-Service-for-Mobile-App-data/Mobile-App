// Represents a login request object used for user authentication.
public class LoginRequest
{
    // The email address of the user attempting to log in.
    public required string email { get; set; }

    // The password of the user attempting to log in.
    public required string password { get; set; }
}
