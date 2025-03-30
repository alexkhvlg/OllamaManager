using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using OllamaManager.Enums;
using OllamaManager.Helpers;
using OllamaManager.Services;
using OllamaManager.Views;
using OllamaSharp;
using OllamaSharp.Models;

namespace OllamaManager.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private OllamaApiClient _ollama;
    private readonly ConfigService _configService;
    private readonly System.Timers.Timer _statusUpdateTimer;
    private readonly SemaphoreSlim _updateStatusSemaphore = new(1, 1);
    private readonly SemaphoreSlim _loadModelsSemaphore = new(1, 1);
    private readonly Dictionary<string, OllamaManager.Models.ModelInfo> _downloadingModels = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public ObservableCollection<OllamaManager.Models.ModelInfo> Models { get; set; } = new();

    [ObservableProperty]
    private string? windowTitle;

    [ObservableProperty]
    private string? ollamaUrl = string.Empty;

    [ObservableProperty]
    private OllamaManager.Models.ModelInfo? selectedModel;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private string ollamaVersion = string.Empty;

    [ObservableProperty]
    private int statusUpdateInterval = 5;

    [ObservableProperty]
    private string modelToDownload = string.Empty;

    public IAsyncRelayCommand WindowLoadedCommandAsync { get; }
    public IRelayCommand SaveConfigCommand { get; }
    public IAsyncRelayCommand TestConnectionCommandAsync { get; }
    public IAsyncRelayCommand ShowModelInfoCommandAsync { get; }
    public IAsyncRelayCommand DeleteModelCommandAsync { get; }
    public IRelayCommand<object?> WindowClosingCommand { get; }
    public IAsyncRelayCommand DownloadModelCommandAsync { get; }

    public MainWindowViewModel()
    {
        WindowTitle = "Ollama Manager";

        _configService = App.Current.Services.GetRequiredService<ConfigService>();
        _configService.Load();

        OllamaUrl = _configService.Config.OllamaUrl;
        StatusUpdateInterval = _configService.Config.StatusUpdateIntervalSeconds;

        _statusUpdateTimer = new System.Timers.Timer();
        _statusUpdateTimer.Elapsed += async (s, e) => await UpdateOllamaStatus();
        _statusUpdateTimer.Interval = StatusUpdateInterval * 1000;

        WindowLoadedCommandAsync = new AsyncRelayCommand(WindowLoadedAsync);
        SaveConfigCommand = new RelayCommand(SaveConfig);
        TestConnectionCommandAsync = new AsyncRelayCommand(TestOllamaConnection);
        ShowModelInfoCommandAsync = new AsyncRelayCommand(ShowModelInfo);
        DeleteModelCommandAsync = new AsyncRelayCommand(DeleteModel);
        WindowClosingCommand = new RelayCommand<object?>(WindowClosing);
        DownloadModelCommandAsync = new AsyncRelayCommand(DownloadModel);
    }


    private void SaveConfig()
    {
        _configService.Config.OllamaUrl = OllamaUrl;
        _configService.Config.StatusUpdateIntervalSeconds = StatusUpdateInterval;
        _configService.Save();

        _statusUpdateTimer.Interval = StatusUpdateInterval * 1000;
    }

    private async Task WindowLoadedAsync()
    {
        try
        {
            InitOllama();

            await LoadModels();
            await UpdateOllamaStatus();

            _statusUpdateTimer.Start();
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке окна: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void InitOllama()
    {
        var uri = new Uri(OllamaUrl);
        _ollama = new OllamaApiClient(uri);
    }

    private async Task LoadModels()
    {
        if (!await _loadModelsSemaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            var currentModels = Models.ToDictionary(m => m.Name);
            Models.Clear();

            var models = await _ollama.ListLocalModelsAsync(_cancellationTokenSource.Token);
            var runningModels = await _ollama.ListRunningModelsAsync(_cancellationTokenSource.Token);
            var runningModelNames = runningModels.Select(m => m.Name).ToHashSet();

            foreach (var model in models)
            {
                var modelInfo = OllamaManager.Models.ModelInfo.FromModel(model);

                if (currentModels.TryGetValue(model.Name, out var existing) && existing.IsDownloading)
                {
                    modelInfo.IsDownloading = true;
                    modelInfo.DownloadProgress = existing.DownloadProgress;
                    modelInfo.Status = Status.Downloading;
                }

                modelInfo.IsRunning = runningModelNames.Contains(model.Name);

                Models.Add(modelInfo);
            }

            foreach (var downloading in _downloadingModels.Values)
            {
                if (!Models.Any(m => m.Name == downloading.Name))
                {
                    Models.Add(downloading);
                }
            }

            var view = CollectionViewSource.GetDefaultView(Models);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load models. {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            _loadModelsSemaphore.Release();
        }
    }

    private async Task UpdateOllamaStatus()
    {
        if (!await _updateStatusSemaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            if (await _ollama.IsRunningAsync())
            {
                var totalBytes = Models.Sum(model => model.Size);
                var version = await _ollama.GetVersionAsync(_cancellationTokenSource.Token);
                OllamaVersion = $"Ollama version: {version}";
                StatusText = $"Total models size: {BytesToHumanReadableFormat.Convert(totalBytes)}";

                var runningModels = await _ollama.ListRunningModelsAsync(_cancellationTokenSource.Token);
                var runningModelNames = runningModels.Select(m => m.Name).ToHashSet();

                foreach (var model in Models)
                {
                    model.IsRunning = runningModelNames.Contains(model.Name);
                }

                OnPropertyChanged(nameof(Models));
            }
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch
        {
            // Игнорируем остальные исключения
        }
        finally
        {
            _updateStatusSemaphore.Release();
        }
    }

    private async Task TestOllamaConnection()
    {
        try
        {
            InitOllama();

            if (await _ollama.IsRunningAsync(_cancellationTokenSource.Token))
            {
                MessageBox.Show("Connection successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Connection failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection failed. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task ShowModelInfo()
    {
        try
        {
            if (SelectedModel is null)
            {
                return;
            }

            var request = new ShowModelRequest
            {
                Model = SelectedModel.Name
            };
            var details = await _ollama.ShowModelAsync(request, _cancellationTokenSource.Token);
            var detailsWindow = App.Current.Services.GetService<ModelDetailsWindow>();
            if (detailsWindow != null)
            {
                detailsWindow.SetModel(SelectedModel.Name, details);
                detailsWindow.ShowDialog();
            }
            else
            {
                throw new Exception("ModelDetailsWindow dependency not found");
            }
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to retrieve model details. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task DeleteModel()
    {
        try
        {
            if (SelectedModel is null)
            {
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete the model '{SelectedModel.Name}' ?", "Delete model", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            await _ollama.DeleteModelAsync(SelectedModel.Name, _cancellationTokenSource.Token);

            await LoadModels();
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to delete model. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void WindowClosing(object? o)
    {
        _cancellationTokenSource.Cancel();
        _statusUpdateTimer?.Stop();
        _statusUpdateTimer?.Dispose();
        _updateStatusSemaphore?.Dispose();
        _loadModelsSemaphore?.Dispose();
    }

    private async Task DownloadModel()
    {
        if (!ValidateModelToDownload(ModelToDownload))
        {
            return;
        }

        var modelInfo = CreateModelInfoForDownload(ModelToDownload);
        RegisterModelForDownload(modelInfo);

        await StartDownloadProcess(modelInfo);

        ModelToDownload = string.Empty;
    }

    private bool ValidateModelToDownload(string modelName)
    {
        if (string.IsNullOrWhiteSpace(modelName))
        {
            MessageBox.Show("Please enter model name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (_downloadingModels.ContainsKey(modelName))
        {
            MessageBox.Show("This model is already downloading", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    private OllamaManager.Models.ModelInfo CreateModelInfoForDownload(string modelName)
    {
        return new OllamaManager.Models.ModelInfo
        {
            Name = modelName,
            IsDownloading = true,
            Status = Status.Downloading,
            DownloadProgress = 0
        };
    }

    private void RegisterModelForDownload(OllamaManager.Models.ModelInfo modelInfo)
    {
        _downloadingModels.Add(modelInfo.Name, modelInfo);
        Models.Add(modelInfo);
    }

    private async Task StartDownloadProcess(OllamaManager.Models.ModelInfo modelInfo)
    {
        try
        {
            await Task.Run(() => DownloadModelAsync(modelInfo));
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start download. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _downloadingModels.Remove(modelInfo.Name);
            await LoadModels();
        }
    }

    private async Task DownloadModelAsync(OllamaManager.Models.ModelInfo modelInfo)
    {
        try
        {
            var progress = CreateDownloadProgressReporter(modelInfo);

            var request = new PullModelRequest
            {
                Model = modelInfo.Name,
                Stream = true
            };

            await foreach (var status in _ollama.PullModelAsync(request, _cancellationTokenSource.Token))
            {
                ((IProgress<int>)progress).Report(Convert.ToInt32(status.Percent));
            }

            await FinishDownloadOnUIThread(modelInfo.Name);
        }
        catch (OperationCanceledException)
        {
            // Прерывание - ничего не делаем
        }
        catch (Exception ex)
        {
            HandleDownloadErrorOnUIThread(modelInfo.Name, ex);
        }
    }

    private Progress<int> CreateDownloadProgressReporter(OllamaManager.Models.ModelInfo modelInfo)
    {
        return new Progress<int>(percent =>
        {
            modelInfo.DownloadProgress = percent;
            Application.Current.Dispatcher.Invoke(() => { OnPropertyChanged(nameof(Models)); });
        });
    }

    private async Task FinishDownloadOnUIThread(string modelName)
    {
        await Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            _downloadingModels.Remove(modelName);

            var modelToRemove = Models.FirstOrDefault(m => m.Name == modelName);
            if (modelToRemove != null)
            {
                Models.Remove(modelToRemove);
            }

            await LoadModels();
        });
    }

    private void HandleDownloadErrorOnUIThread(string modelName, Exception ex)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (ex is OperationCanceledException)
            {
                return;
            }
            MessageBox.Show($"Failed to load model {modelName}. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _downloadingModels.Remove(modelName);
            LoadModels().ConfigureAwait(false);
        });
    }
}
