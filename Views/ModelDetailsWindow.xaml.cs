using System.Windows;
using OllamaManager.ViewModels;
using OllamaSharp.Models;

namespace OllamaManager.Views;
/// <summary>
/// Interaction logic for ModelDetailsWindow.xaml
/// </summary>
public partial class ModelDetailsWindow : Window
{
    public ModelDetailsWindow(ModelDetailsViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }

    public void SetModel(string name, ShowModelResponse model)
    {
        ((ModelDetailsViewModel)DataContext).LoadModel(name, model);
    }
}
