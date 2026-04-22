using GLMS_Core_Prototype.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS_Core_Prototype.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Contract>()
                .HasMany(c => c.ServiceRequests)
                .WithOne(r => r.Contract)
                .HasForeignKey(r => r.ContractId);

            modelBuilder.Entity<ServiceRequest>()
                .Property(r => r.CostUSD)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ServiceRequest>()
                .Property(r => r.CostZAR)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Client>()
                .Property(c => c.ContactDetails)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Client>()
                .Property(c => c.Region)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Contract>()
                .Property(c => c.ServiceLevel)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Contract>()
                .Property(c => c.Region)
                .HasMaxLength(100);

            modelBuilder.Entity<ServiceRequest>()
                .Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}