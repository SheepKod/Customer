using Customer.Domain.Models;
using Customer.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts): DbContext(opts)
{
    public DbSet<IndividualCustomer> RetailCustomers { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<Relation> Relations { get; set; }
    public DbSet<City> Cities { get; set; }
 
    
    // aq xdeba relationebis dasetva da seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        // General Restrictions and stuff
        modelBuilder.Entity<IndividualCustomer>()
            .Property(c=>c.Id)
            .ValueGeneratedOnAdd(); // anu id incrementirdeba axlis damatebisas mara eg xo defaultad isedac mase?

        
        // is ro min 2 unda iyos da qartuli an inglisuri asoebi eg business level ariso
        modelBuilder.Entity<IndividualCustomer>()
            .Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<IndividualCustomer>()
            .Property(c => c.LastName)
            .IsRequired()
            
            .HasMaxLength(50);

        modelBuilder.Entity<IndividualCustomer>()
            .Property(c => c.PersonalId)
            .IsRequired()
            .HasMaxLength(11);

        modelBuilder.Entity<IndividualCustomer>()
            .HasIndex(c => c.PersonalId)
            .IsUnique();

        modelBuilder.Entity<IndividualCustomer>()
            .Property(c => c.DateOfBirth)
            .IsRequired();

        modelBuilder.Entity<PhoneNumber>()
            .Property(c => c.Number)
            .HasMaxLength(50);

        
        // Table Relations
        modelBuilder.Entity<IndividualCustomer>()
            .HasOne<City>()  // Veubnebit ro am customers sheesabameba 1 qalaqi
            .WithMany() // veubnebit ro tito qalaqs sheileba bevri customer sheesabamebodes
            .HasForeignKey(c => c.CityId); // foreign key
        modelBuilder.Entity<PhoneNumber>()
            .HasOne<IndividualCustomer>()
            .WithMany(x=> x.PhoneNumbers) // es miemarteba retailcustomers
            .HasForeignKey(p => p.IndividualCustomerId);
            
        modelBuilder.Entity<Relation>()
            .HasOne<IndividualCustomer>()
            .WithMany()
            .HasForeignKey(r => r.RelatedCustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Seeding
        modelBuilder.Entity<City>().HasData(CustomersData.Cities);

    }
}