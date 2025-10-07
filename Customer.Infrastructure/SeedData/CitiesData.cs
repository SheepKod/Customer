using Customer.Domain.Models;

namespace Customer.Infrastructure.SeedData;

public static class CitiesData
{
    public static readonly List<City> Cities =
    [

        // Georgia
        new() { Id = 1, Name = "Tbilisi", Country = "Georgia" },
        new() { Id = 2, Name = "Batumi", Country = "Georgia" },
        new() { Id = 3, Name = "Kutaisi", Country = "Georgia" },

        // USA
        new() { Id = 4, Name = "New York", Country = "USA" },
        new() { Id = 5, Name = "San Francisco", Country = "USA" },

        // Germany
        new() { Id = 6, Name = "Munich", Country = "Germany" },
        new() { Id = 7, Name = "Berlin", Country = "Germany" },

        // United Kingdom
        new() { Id = 8, Name = "London", Country = "UK" },
        new() { Id = 9, Name = "Manchester", Country = "UK" },

        // Japan
        new() { Id = 10, Name = "Tokyo", Country = "Japan" },
        new() { Id = 11, Name = "Osaka", Country = "Japan" },

        // France
        new() { Id = 12, Name = "Paris", Country = "France" },
        new() { Id = 13, Name = "Lyon", Country = "France" },

        // Australia
        new() { Id = 14, Name = "Sydney", Country = "Australia" },
        new() { Id = 15, Name = "Melbourne", Country = "Australia" },
    ];
}