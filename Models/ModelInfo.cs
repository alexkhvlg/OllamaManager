using System.ComponentModel;
using OllamaManager.Enums;
using OllamaSharp.Models;

namespace OllamaManager.Models;

public class ModelInfo : INotifyPropertyChanged
{
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; }
    public Details? Details { get; set; }

    private bool _isDownloading;
    public bool IsDownloading
    {
        get => _isDownloading;
        set
        {
            _isDownloading = value;
            OnPropertyChanged(nameof(IsDownloading));
        }
    }

    private int _downloadProgress;
    public int DownloadProgress
    {
        get => _downloadProgress;
        set
        {
            _downloadProgress = value;
            OnPropertyChanged(nameof(DownloadProgress));
        }
    }

    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            OnPropertyChanged(nameof(IsRunning));
        }
    }

    public Status Status { get; set; }

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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}