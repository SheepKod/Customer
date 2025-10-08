using System.Globalization;

namespace Customer.API.Middlewares;

public class LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
{
    private static readonly string[] SupportedCultures = { "ka", "en", "ru", "ge" };
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

            logger.LogWarning(ex.Message);
        }
        
        await next(context);
    }
}