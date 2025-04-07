
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OllamaManager.Services;
using OllamaManager.ViewModels;
using OllamaManager.Views;

namespace OllamaManager;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public static new App Current => (App)Application.Current;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services
    {
        get;
    }

    public App()
    {
        Services = ConfigureServices();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = new MainWindow(Services.GetRequiredService<MainWindowViewModel>());
        mainWindow.Show();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<DetailsViewModel>();

        services.AddTransient<DetailsWindow>();
        services.AddTransient<DetailsWindow>();

        services.AddSingleton<ConfigService>();

        return services.BuildServiceProvider();
    }
}

