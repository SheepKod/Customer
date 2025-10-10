namespace Customer.Application.Constants;

public static class LocalizationConstants
{
    public static readonly IReadOnlySet<string> SupportedCultures = new HashSet<string> 
    { 
        "en", 
        "ka", 
    };

    public const string DefaultCulture = "ka";
}