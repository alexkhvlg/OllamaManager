using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using OllamaSharp.Models;

namespace OllamaManager.ViewModels;
public partial class ModelDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    private string? license;

    [ObservableProperty]
    private string? modelfile;

    [ObservableProperty]
    private string? parameters;

    [ObservableProperty]
    private string? template;

    [ObservableProperty]
    private string? system;

    [ObservableProperty]
    private Details details = new();

    [ObservableProperty]
    private ModelInfo modelInfo = new();

    [ObservableProperty]
    private string? infoExtraInfo;

   [ObservableProperty]
    private string? projectorExtraInfo;

    [ObservableProperty]
    private string windowTitle;

    [ObservableProperty]
    private string json;

    // Конструктор по умолчанию
    public ModelDetailsViewModel()
    {
    }

    public void LoadModel(string name, ShowModelResponse model)
    {
        WindowTitle = name;
        License = model.License;
        Modelfile = model.Modelfile;
        Parameters = model.Parameters;
        Template = model.Template;
        System = model.System;
        Details = model.Details ?? new Details();
        ModelInfo = model.Info ?? new ModelInfo();

        if (model?.Info?.ExtraInfo is not null)
        {
            InfoExtraInfo = JsonConvert.SerializeObject(model.Info.ExtraInfo, Formatting.Indented);
        }

        if (model?.Projector?.ExtraInfo is not null)
        {
            ProjectorExtraInfo = JsonConvert.SerializeObject(model.Projector.ExtraInfo, Formatting.Indented);
        }
        Json = JsonConvert.SerializeObject(model, Formatting.Indented);
    }
}
