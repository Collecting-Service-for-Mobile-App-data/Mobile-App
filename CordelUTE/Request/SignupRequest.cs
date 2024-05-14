using MauiApp1;

namespace CordelUTE
{
    /// <summary>
    /// Represents a signup request object used for creating a new user account.
    /// </summary>
    public class SignupRequest
    {
        /// <summary>
        /// The company information associated with the new user.
        /// </summary>
        public CompanyRequest company { get; set; }

        /// <summary>
        /// The email address of the new user.
        /// </summary>
        public required string email { get; set; }

        /// <summary>
        /// The password of the new user.
        /// </summary>
        public required string password { get; set; }
    }
}
