namespace MauiApp1
{
    /// <summary>
    /// Represents a user entity within the application.
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public required long id { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public required string email { get; set; }

        /// <summary>
        /// The unique identifier of the company the user belongs to.
        /// </summary>
        public required long companyId { get; set; }
    }
}
