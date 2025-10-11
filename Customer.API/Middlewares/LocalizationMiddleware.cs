using System.Collections.ObjectModel;
using System.Globalization;
using Customer.Application.Constants;

namespace Customer.API.Middlewares;

public class LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var requestedCulture = context.Request.Headers["Accept-Language"].ToString();
        var cultureLang = LocalizationConstants.DefaultCulture;
        if (LocalizationConstants.SupportedCultures.Contains(requestedCulture))
        {
            cultureLang = requestedCulture;
        }

        try
        {
            var culture = new CultureInfo(cultureLang);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            logger.LogDebug($"Culture set to {culture.Name}");
        }
        catch (CultureNotFoundException ex)
        {
            var fallbackCulture = new CultureInfo(LocalizationConstants.DefaultCulture);
            CultureInfo.CurrentCulture = fallbackCulture;
            CultureInfo.CurrentUICulture = fallbackCulture;

            logger.LogWarning(
                $"Falling Back To Default Language,Requested Culture Not Supported : {requestedCulture} {ex.Message}");
        }

        await next(context);
    }
}