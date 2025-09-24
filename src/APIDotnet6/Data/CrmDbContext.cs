using Microsoft.EntityFrameworkCore;
using APIDotnet6.Models;

namespace APIDotnet6.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Opportunity> Opportunities { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Status).HasDefaultValue("Active");
            });
            
            // Configure Contact
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Customer)
                      .WithMany(e => e.Contacts)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure Opportunity
            modelBuilder.Entity<Opportunity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Customer)
                      .WithMany(e => e.Opportunities)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.EstimatedValue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasDefaultValue("Open");
            });
            
            // Seed some sample data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Phone = "555-123-4567",
                    Company = "Acme Corp",
                    JobTitle = "CEO",
                    Address = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA",
                    CreatedDate = DateTime.UtcNow.AddDays(-30),
                    Status = "Active"
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Phone = "555-987-6543",
                    Company = "TechStart Inc",
                    JobTitle = "CTO",
                    Address = "456 Oak Ave",
                    City = "San Francisco",
                    State = "CA",
                    PostalCode = "94102",
                    Country = "USA",
                    CreatedDate = DateTime.UtcNow.AddDays(-15),
                    Status = "Active"
                },
                new Customer
                {
                    Id = 3,
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Email = "mike.johnson@example.com",
                    Phone = "555-456-7890",
                    Company = "Global Solutions",
                    JobTitle = "VP Sales",
                    Address = "789 Pine St",
                    City = "Chicago",
                    State = "IL",
                    PostalCode = "60601",
                    Country = "USA",
                    CreatedDate = DateTime.UtcNow.AddDays(-45),
                    Status = "Active"
                }
            );
            
            // Seed Opportunities
            modelBuilder.Entity<Opportunity>().HasData(
                new Opportunity
                {
                    Id = 1,
                    CustomerId = 1,
                    Title = "Software License Deal",
                    Description = "Annual software licensing agreement",
                    EstimatedValue = 50000.00m,
                    Probability = 75,
                    Stage = "Negotiation",
                    ExpectedCloseDate = DateTime.UtcNow.AddDays(30),
                    AssignedTo = "Sales Rep 1",
                    CreatedDate = DateTime.UtcNow.AddDays(-20),
                    Status = "Open"
                },
                new Opportunity
                {
                    Id = 2,
                    CustomerId = 2,
                    Title = "Cloud Migration Project",
                    Description = "Complete cloud infrastructure migration",
                    EstimatedValue = 125000.00m,
                    Probability = 60,
                    Stage = "Proposal",
                    ExpectedCloseDate = DateTime.UtcNow.AddDays(45),
                    AssignedTo = "Sales Rep 2",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    Status = "Open"
                }
            );
            
            // Seed Contacts
            modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = 1,
                    CustomerId = 1,
                    Type = "Phone",
                    Subject = "Initial Discovery Call",
                    Description = "Discussed current software needs and pain points",
                    ContactDate = DateTime.UtcNow.AddDays(-25),
                    Status = "Completed",
                    ContactedBy = "Sales Rep 1",
                    CreatedDate = DateTime.UtcNow.AddDays(-25)
                },
                new Contact
                {
                    Id = 2,
                    CustomerId = 1,
                    Type = "Email",
                    Subject = "Follow-up Proposal",
                    Description = "Sent detailed proposal and pricing information",
                    ContactDate = DateTime.UtcNow.AddDays(-15),
                    Status = "Completed",
                    ContactedBy = "Sales Rep 1",
                    CreatedDate = DateTime.UtcNow.AddDays(-15)
                },
                new Contact
                {
                    Id = 3,
                    CustomerId = 2,
                    Type = "Meeting",
                    Subject = "Technical Requirements Review",
                    Description = "On-site meeting to review technical requirements",
                    ContactDate = DateTime.UtcNow.AddDays(-5),
                    Status = "Completed",
                    ContactedBy = "Sales Rep 2",
                    CreatedDate = DateTime.UtcNow.AddDays(-8)
                }
            );
        }
    }
}