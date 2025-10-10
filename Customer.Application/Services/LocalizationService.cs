using System.Globalization;
using System.Text.Json;
using Customer.Application.Constants;
using Microsoft.AspNetCore.Hosting;

namespace Customer.Application.Services;

public class LocalizationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _localizations = new();

    public LocalizationService(IWebHostEnvironment env)
    {
        LoadLocalizations(env);
    }

    private void LoadLocalizations(IWebHostEnvironment env)
    {
        // This gets the Customer.API directory
        var resourcePath = Path.Combine(env.ContentRootPath, "..", "Customer.Application", "Resources");
        
        foreach (var culture in LocalizationConstants.SupportedCultures)
        {
            var filePath = Path.Combine(resourcePath, $"ValidationMessages.{culture}.json");
            
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var messages = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                
                if (messages != null)
                {
                    _localizations[culture] = messages;
                }
            }
        }
    }

    public string GetString(string key)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        if (_localizations.TryGetValue(culture, out var messages) && 
            messages.TryGetValue(key, out var message))
        {
            return message;
        }
        
        if (_localizations.TryGetValue(LocalizationConstants.DefaultCulture, out var defaultMessages) && 
            defaultMessages.TryGetValue(key, out var defaultMessage))
        {
            return defaultMessage;
        }
        
        return key;
    }

    public string this[string key] => GetString(key);
}