using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Infrastructure.SeedData;

public static class CustomersData
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
    
    public static readonly List<IndividualCustomer> Customers = new()
    {
        new IndividualCustomer
        {
            Id = 1,
            FirstName = "Marry",
            LastName = "KOP",
            Gender = Gender.Female,
            PersonalId = "01001002144",
            DateOfBirth = new DateTime(1995, 12, 10),
            CityId = 1,
            ImageKey = Guid.NewGuid()
        },
        new IndividualCustomer
        {
            Id = 2,
            FirstName = "Tun",
            LastName = "Sahur",
            Gender = Gender.Male,
            PersonalId = "01001002145",
            DateOfBirth = new DateTime(1992, 6, 17),
            CityId = 2,
            ImageKey = Guid.NewGuid()
        }
    };

    public static readonly List<PhoneNumber> PhoneNumbers = new()
    {
        new PhoneNumber { Id = 1, Number = "555111222", Type = PhoneType.Mobile, CustomerId = 1 },
        new PhoneNumber { Id = 2, Number = "322123456", Type = PhoneType.Home, CustomerId = 1 },
        new PhoneNumber { Id = 3, Number = "555333444", Type = PhoneType.Mobile, CustomerId = 2 }
    };

    public static readonly List<Relation> Relations = new()
    {
        // Nino is a colleague of Giorgi
        new Relation
        {
            Id = 1,
            CustomerId = 1,
            RelatedCustomerId = 2,
            Type = RelationType.Colleague
        },
        // Giorgi is an acquaintance of Nino
        new Relation
        {
            Id = 2,
            CustomerId = 2,
            RelatedCustomerId = 1,
            Type = RelationType.Acquaintance
        }
    };

}