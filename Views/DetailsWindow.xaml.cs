using System.Windows;
using OllamaManager.ViewModels;
using OllamaSharp.Models;

namespace OllamaManager.Views;
/// <summary>
/// Interaction logic for ModelDetailsWindow.xaml
/// </summary>
public partial class DetailsWindow : Window
{
    public DetailsWindow(DetailsViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }

    public void SetModel(string name, ShowModelResponse model)
    {
        ((DetailsViewModel)DataContext).LoadModel(name, model);
    }
}
