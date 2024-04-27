using System;
using MauiApp1;

namespace CordelUTE
{
    // Represents a signup request object used for creating a new user account.
public class SignupRequest
{
    // The company information associated with the new user.
    public Company company { get; set; }

    // The email address of the new user.
    public required string email { get; set; }

    // The password of the new user.
    public required string password { get; set; }
}

}
