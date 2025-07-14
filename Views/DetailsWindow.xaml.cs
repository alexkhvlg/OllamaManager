using System.Windows;
using OllamaManager.ViewModels;
using OllamaSharp.Models;

namespace OllamaManager.Views;
/// <summary>
/// Interaction logic for ModelDetailsWindow.xaml
/// </summary>
public partial class DetailsWindow : Window
{
    private readonly DetailsWindowViewModel _detailsWindowViewModel;

    public DetailsWindow(DetailsWindowViewModel viewModel)
    {
        InitializeComponent();

        _detailsWindowViewModel = viewModel;

        DataContext = viewModel;
    }

    public void SetModel(string name, ShowModelResponse model)
    {
        _detailsWindowViewModel.LoadModel(name, model);
    }
}
