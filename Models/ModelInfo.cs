using CommunityToolkit.Mvvm.ComponentModel;
using OllamaManager.Enums;
using OllamaManager.Helpers;
using OllamaSharp.Models;

namespace OllamaManager.Models;

public partial class ModelInfo : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private long size;

    [ObservableProperty]
    private string gpuPercent = string.Empty;

    [ObservableProperty]
    private bool isDownloading;

    [ObservableProperty]
    private int downloadProgress;

    [ObservableProperty]
    private bool isRunning;

    [ObservableProperty]
    private Status status;

    [ObservableProperty]
    private string? parentModel;

    [ObservableProperty]
    private string format = string.Empty;

    [ObservableProperty]
    private string family = string.Empty;

    [ObservableProperty]
    private string[]? families;

    [ObservableProperty]
    private decimal parameterSize = 0;

    [ObservableProperty]
    private string quantizationLevel = string.Empty;

    [ObservableProperty]
    private int contextLength = 0;

    public static ModelInfo FromModel(Model model)
    {
        return new ModelInfo
        {
            Name = model.Name,
            Size = model.Size,
            Status = Status.Ready,
            ParentModel = model.Details?.ParentModel,
            Format = model.Details?.Format ?? string.Empty,
            Family = model.Details?.Family ?? string.Empty,
            Families = model.Details?.Families,
            ParameterSize = ParameterSizeToDecimal.Convert(model.Details?.ParameterSize),
            ContextLength = 0
        };
    }
}