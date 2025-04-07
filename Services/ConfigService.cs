using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace OllamaManager.Services;

public class ConfigService
{
    private readonly string _configFileName;

    public Config Config { get; set; } = new();

    public ConfigService()
    {
        _configFileName = "ollama_manager.json";
    }

    public ConfigService(string configFileName)
    {
        _configFileName = configFileName;
    }

    public void Load()
    {

        try
        {
            // App Settings
            var content = File.ReadAllText(_configFileName);
            if (content is not null)
            {
                Config = JsonConvert.DeserializeObject<Config>(content) ?? new ();
            }

            // Ollama Configuration
            Config.ollamaHost = Environment.GetEnvironmentVariable("OLLAMA_HOST", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaModels = Environment.GetEnvironmentVariable("OLLAMA_MODELS", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaOrigins = Environment.GetEnvironmentVariable("OLLAMA_ORIGINS", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaContextLength = Environment.GetEnvironmentVariable("OLLAMA_CONTEXT_LENGTH", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaKeepAlive = Environment.GetEnvironmentVariable("OLLAMA_KEEP_ALIVE", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaMaxQueue = Environment.GetEnvironmentVariable("OLLAMA_MAX_QUEUE", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaMaxLoadedModels = Environment.GetEnvironmentVariable("OLLAMA_MAX_LOADED_MODELS", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaNumParallel = Environment.GetEnvironmentVariable("OLLAMA_NUM_PARALLEL", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaFlashAttention = Environment.GetEnvironmentVariable("OLLAMA_FLASH_ATTENTION", EnvironmentVariableTarget.Machine) ?? string.Empty;
            Config.ollamaKvCacheType = Environment.GetEnvironmentVariable("OLLAMA_KV_CACHE_TYPE", EnvironmentVariableTarget.Machine) ?? string.Empty;

        }
        catch
        {
            Config = new();
        }
    }

    public void Save()
    {
        // App Settings
        var content = JsonConvert.SerializeObject(Config);
        File.WriteAllText(_configFileName, content);

        // Ollama Configuration
        Environment.SetEnvironmentVariable("OLLAMA_HOST", Config.ollamaHost, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_MODELS", Config.ollamaModels, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_ORIGINS", Config.ollamaOrigins, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_CONTEXT_LENGTH", Config.ollamaContextLength, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_KEEP_ALIVE", Config.ollamaKeepAlive, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_MAX_QUEUE", Config.ollamaMaxQueue, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_MAX_LOADED_MODELS", Config.ollamaMaxLoadedModels, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_NUM_PARALLEL", Config.ollamaNumParallel, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_FLASH_ATTENTION", Config.ollamaFlashAttention, EnvironmentVariableTarget.Machine);
        Environment.SetEnvironmentVariable("OLLAMA_KV_CACHE_TYPE", Config.ollamaKvCacheType, EnvironmentVariableTarget.Machine);
    }
}

public class Config
{
    ///
    /// Connection Settings
    ///
    public string OllamaUrl { get; set; } = "http://127.0.0.1:11434";

    public int StatusUpdateIntervalSeconds { get; set; } = 5;

    ///
    /// Ollama Configuration
    ///
    public string ollamaHost { get; set; } = string.Empty;

    public string ollamaModels { get; set; }= string.Empty;

    public string ollamaOrigins { get; set; } = string.Empty;

    public string ollamaContextLength { get; set; } = string.Empty;

    public string ollamaKeepAlive { get; set; } = string.Empty;

    public string ollamaMaxQueue { get; set; } = string.Empty;

    public string ollamaMaxLoadedModels { get; set; } = string.Empty;

    public string ollamaNumParallel { get; set; } = string.Empty;

    public string ollamaFlashAttention { get; set; } = string.Empty;

    public string ollamaKvCacheType { get; set; } = string.Empty;
}