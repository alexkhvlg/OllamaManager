using System.Windows;
using OllamaManager.ViewModels;

namespace OllamaManager.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainViewModel)
    {
        InitializeComponent();

        DataContext = mainViewModel;
    }
}