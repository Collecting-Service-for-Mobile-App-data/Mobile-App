namespace CordelUTE
{
    /// <summary>
    /// Represents the main application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Default constructor for the App class. This constructor
        /// is called automatically when the application starts.
        /// </summary>
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
