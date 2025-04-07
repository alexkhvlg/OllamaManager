using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using OllamaManager.Enums;
using OllamaSharp.Models;

namespace OllamaManager.Models;

public partial class ModelInfo : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private long size;

    [ObservableProperty]
    private Details? details;

    [ObservableProperty]
    private string gpuPercent;

    [ObservableProperty]
    private bool isDownloading;

    [ObservableProperty]
    private int downloadProgress;

    [ObservableProperty]
    private bool isRunning;

    [ObservableProperty]
    private Status status;

    public static ModelInfo FromModel(Model model)
    {
        return new ModelInfo
        {
            Name = model.Name,
            Size = model.Size,
            Details = model.Details,
            Status = Status.Ready
        };
    }
}