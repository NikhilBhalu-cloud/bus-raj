using Microsoft.EntityFrameworkCore;
using BusReservationApi.Models;

namespace BusReservationApi.Data
{
    public class BusReservationContext : DbContext
    {
        public BusReservationContext(DbContextOptions<BusReservationContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<BusRoute> Routes { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Stations
            modelBuilder.Entity<Station>().HasData(
                new Station { Id = 1, Name = "Ahmedabad" },
                new Station { Id = 2, Name = "Baroda" },
                new Station { Id = 3, Name = "Bharuch" },
                new Station { Id = 4, Name = "Surat" },
                new Station { Id = 5, Name = "Mumbai" }
            );

            // Seed Routes
            modelBuilder.Entity<BusRoute>().HasData(
                new BusRoute 
                { 
                    Id = 1, 
                    Name = "Ahmedabad to Mumbai", 
                    Stations = "Ahmedabad,Baroda,Bharuch,Surat,Mumbai",
                    TotalSeats = 60
                },
                new BusRoute 
                { 
                    Id = 2, 
                    Name = "Ahmedabad to Surat", 
                    Stations = "Ahmedabad,Baroda,Bharuch,Surat",
                    TotalSeats = 60
                }
            );
        }
    }
}
