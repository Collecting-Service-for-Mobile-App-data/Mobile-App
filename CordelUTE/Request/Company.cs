namespace MauiApp1
{
    /// <summary>
    /// Represents a company entity within the application.
    /// </summary>
    public class CompanyRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the company.
        /// </summary>
        public required int id { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public required string name { get; set; }
    }
}
