using System.Globalization;

namespace Customer.API.Middlewares;

public class LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
{
    private static readonly string[] SupportedCultures = { "ka", "en", "ru", "ge" };
    private const string DefaultCulture = "ka";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var requestedCulture = context.Request.Headers["Accept-Language"].ToString();
        var culture = new CultureInfo(DefaultCulture);
        if (SupportedCultures.Contains(requestedCulture))
        {
            culture = new CultureInfo(requestedCulture);
        }
        
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        await next(context);
    }
}