using System.IO;
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
            var content = File.ReadAllText(_configFileName);
            if (content is not null)
            {
                Config = JsonConvert.DeserializeObject<Config>(content) ?? new ();
            }
        }
        catch
        {
            Config = new();
        }
    }

    public void Save()
    {
        var content = JsonConvert.SerializeObject(Config);
        File.WriteAllText(_configFileName, content);
    }
}

public class Config
{
    public string OllamaUrl { get; set; } = "http://127.0.0.1:11434";

    public int StatusUpdateIntervalSeconds { get; set; } = 5;
}