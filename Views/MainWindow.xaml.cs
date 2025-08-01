using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OllamaManager.ViewModels;

namespace OllamaManager.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<MainWindowViewModel>();
    }

    public MainWindow(MainWindowViewModel mainViewModel) : this()
    {
    }
}