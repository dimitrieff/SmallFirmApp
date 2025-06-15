using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Data
{
    public class ApplicationDbContext: IdentityDbContext

    {
        

        public DbSet<Client> Client { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Consumative> Consumative { get; set; }
        public DbSet<ExtraService> ExtraService { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<Visite> Visite { get; set; }
        public DbSet<Deliveries> Deliveries { get; set; }
        public DbSet<ProcessedService> ProcessedService { get; set; }
        public DbSet<ForDelivery> ForDelivery { get; set; }
        public DbSet<InvoiceDDS> InvoiceDDS { get; set; }
        public DbSet<InvoiceSimple> InvoiceSimple { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Name = "---",
                },
                new Client
                {
                    Id = 2,
                    Name = "Client1",
                }
                );
            modelBuilder.Entity<Consumative>().HasData(
               new Consumative
               {
                   Id = 1,
                   Name = "Consumative1",
                   Price = 6.80
               },
                new Consumative
                {
                    Id = 2,
                    Name = "Consumative2",
                    Price = 6.80
                }
               );
            modelBuilder.Entity<Staff>().HasData(
               new Staff
               {
                   Id = 1,
                   Name = "---"
               },
               new Staff
               {
                   Id = 2,
                   Name = "Staff1"
               }
               );
            modelBuilder.Entity<AddOn>().HasData(
               new AddOn
               {
                   Id = 1,
                   Name = "---"
               },
               new AddOn
               {
                   Id = 2,
                   Name = "AddOn1"
               }
               );
            modelBuilder.Entity<ExtraService>().HasData(
               new ExtraService
               {
                   Id = 1,
                   Name = "ExtraService1",
                   ExtraPrice = 20.00
               },
                new ExtraService
                {
                    Id = 2,
                    Name = "ExtraService2",
                    ExtraPrice = 20.00
                }
               );
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
