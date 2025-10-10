namespace Customer.Application.Constants;

public static class LocalizationConstants
{
    public static readonly IReadOnlySet<string> SupportedCultures= new HashSet<string>{"ka", "en"};
    public const string DefaultCulture = "ka";
}