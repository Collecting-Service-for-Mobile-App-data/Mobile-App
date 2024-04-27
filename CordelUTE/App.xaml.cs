namespace CordelUTE;

public partial class App : Application
{
    // Default constructor for the App class. This constructor
    // is called automatically when the application starts.
    public App()
    {
        // Initialize the application components defined in the
        // XAML file (likely App.xaml) associated with this class.
        InitializeComponent();

        // Set the main page of the application to a new instance
        // of the AppShell component. This defines the initial screen
        // the user sees when the application launches.
        MainPage = new AppShell();
    }
}
