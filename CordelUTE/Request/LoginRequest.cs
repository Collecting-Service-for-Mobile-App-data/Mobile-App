/// <summary>
/// Represents a login request object used for user authentication.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The email address of the user attempting to log in.
    /// </summary>
    public required string email { get; set; }

    /// <summary>
    /// The password of the user attempting to log in.
    /// </summary>
    public required string password { get; set; }
}
