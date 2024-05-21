# Cordel Mobile App - Bachelor Project at NTNU

Welcome to the Cordel Mobile App repository, a part of a bachelor project in Computer Engineering at the Norwegian University of Science and Technology (NTNU). This repository is maintained by a student team tasked with developing a mobile application as a component of a testing environment for the service we are creating for Cordel. The service is designed to report errors to developers in the exact state the user encountered the error.

## Project Description

The Cordel Mobile App is designed to facilitate error reporting by allowing users to send error logs directly from their mobile devices. The error logs, in the form of raw SQL files, are transmitted to the Cordel developers' admin website. Developers can then access and download these files to diagnose the reported issues, making the troubleshooting process efficient and user-centric.



## Project Structure

```
CordelUTE/
├── Dependencies/
├── Properties/
├── Database/
├── DatabaseTempFiles/
├── Platforms/
├── Request/
├── Resources/
├── Service/
├── Tests/
├── App.xaml
├── AppShell.xaml
├── LoginPage.xaml
├── MainPage.xaml
├── MauiProgram.cs
└── SignupPage.xaml
```


- `App.xaml` and `App.xaml.cs`: The application's main entry point.
- `AppShell.xaml` and `AppShell.xaml.cs`: Defines the app's shell.
- `LoginPage.xaml` and `LoginPage.xaml.cs`: The login page of the app.
- `MainPage.xaml` and `MainPage.xaml.cs`: The main page of the app.
- `SignupPage.xaml` and `SignupPage.xaml.cs`: The signup page of the app.
- `Database/`: Contains the database related files.
- `Platforms/`: Contains platform-specific code for Android and iOS.
- `Properties/`: Contains the application's property files.
- `Request/`: Contains the request related files.
- `Resources/`: Contains the application's resources.
- `Service/`: Contains the service related files.
- `Tests/`: Contains the unit tests for the application.

## Features

- **User Authentication**: Secure login and signup functionality.
- **Responsive Design**: Optimized for various screen sizes.
- **Database Integration**: Seamless interaction with the backend database.
- **API Requests**: Efficient handling of API requests and responses.
- **Cross-Platform Support**: Runs on both Android and iOS.


## Technologies Used
* .NET MAUI: For cross-platform mobile app development.
* SQLite: For database management.
* XAML: For UI design.
* C#: Primary programming language.

## Installation and Setup
To get a local copy up and running, follow these steps:

### Mac info
To access the storage and running the app on ios an apple developer account is neccasry. Wihtout this you will not be able to login or send errors. This will not be a problem if the app is deployed to app store.

### Clone the repository:
- git clone https://github.com/yourusername/cordel-mobile-app.git

### Navigate to the project directory:
- cd cordel-mobile-app

### Restore dependencies:
- dotnet restore

### Build the project:
- dotnet build

### Run the project:
- dotnet run
