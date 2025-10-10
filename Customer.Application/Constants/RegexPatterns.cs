namespace Customer.Application.Constants;

public static class RegexPatterns
{
    public const string LatinLettersOnly = @"^[a-zA-Z]+$";
    public const string GeorgianLettersOnly = @"^[ა-ჰ]+$";
    public const string NumericOnly = @"^\d+$";
    public const string PhoneNumber = @"^\d{4,50}$";
}
