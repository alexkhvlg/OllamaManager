# OllamaManager

OllamaManager is a Windows Presentation Foundation (WPF) application designed to manage models using the Ollama API client. The application provides functionality to view, download, and manage model details.

## Features

- **Model Management**: Load and display local models along with their details.
- **Downloading Models**: Initiate downloads for specific models, with progress tracking.
- **Connection Testing**: Test connection to the Ollama service to ensure functionality.
- **Configuration Management**: Provides configurable settings for the Ollama server URL and status update intervals.
- **Details View**: Detailed insights into the models, including architecture, file type, parameters, and quantization details.

## Requirements

- .NET 8.0 Runtime
- Windows OS (due to WPF usage)

## Installation

1. Clone this repository:

   ```bash
   git clone <repository-url>
   ```

2. Navigate to the project directory:

   ```bash
   cd OllamaManager
   ```

3. Restore the dependencies:

   ```bash
   dotnet restore
   ```

4. Build the project:

   ```bash
   dotnet build
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

## Configuration

- Modify `ollama_manager.json` to set the Ollama server URL and status update intervals.
- Use the in-application settings to adjust the configuration and automatically save changes.

## Usage

- On startup, the application connects to the configured Ollama server, loads available models, and displays them.
- You can download models, view model details, and check the running status of each model.

## Contributing

Contributions to enhance the OllamaManager are welcome. Feel free to submit issues and pull requests.

## License

This project is licensed under the MIT License.

## Acknowledgments

- Utilizes the [OllamaSharp](https://www.nuget.org/packages/OllamaSharp/) package for interacting with the Ollama API.
- Implements the [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm/) package for MVVM framework support.