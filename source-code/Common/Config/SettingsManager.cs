using System.Configuration;

namespace Common.Config;

public class SettingsManager : ISettingsManager
{
    public string Get(string key)
    {
        try
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] ?? string.Empty;
        }
        catch (ConfigurationErrorsException)
        {
            Console.WriteLine("Error reading config file");
            return string.Empty;
        }
    }
}