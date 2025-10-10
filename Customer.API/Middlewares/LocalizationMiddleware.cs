using System.Collections.ObjectModel;
using System.Globalization;

namespace Customer.API.Middlewares;

public class LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
{
    private IReadOnlySet<string> SupportedCultures => new HashSet<string> { "ka", "en", "ru", "ge" };

    private const string DefaultCulture = "ka";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var requestedCulture = context.Request.Headers["Accept-Language"].ToString();
        var cultureLang = DefaultCulture;
        if (SupportedCultures.Contains(requestedCulture))
        {
            
            cultureLang = requestedCulture;
            
        }

        try
        {
            var culture = new CultureInfo(cultureLang);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            logger.LogInformation($"Culture set to {culture.Name}");
        }
        catch (CultureNotFoundException ex)
        {
            var fallbackCulture = new CultureInfo(DefaultCulture);
            CultureInfo.CurrentCulture = fallbackCulture;
            CultureInfo.CurrentUICulture = fallbackCulture;

            logger.LogWarning($"Falling Back To Default Language,Requested Culture Not Supported : {requestedCulture} {ex.Message}");
        }
        
        await next(context);
    }
}