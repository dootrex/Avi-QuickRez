using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using rolesDemoSSD.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using rolesDemoSSD.Models;

namespace rolesDemoSSD.Data
{


    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }



        public DbSet<rolesDemoSSD.ViewModels.RoleVM> RoleVM { get; set; }

        public DbSet<rolesDemoSSD.ViewModels.UserVM> UserVM { get; set; }

        public DbSet<rolesDemoSSD.ViewModels.UserRoleVM> UserRoleVM { get; set; }


        // Define entity collections.
        public DbSet<Restaurant> restaurants { get; set; }
        public DbSet<RestaurantTable> restaurantTables { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<Customer> customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.RestaurantTable)
                .WithMany(r => r.Reservations)
                .HasForeignKey(fk => new { fk.RestaurantTableID })
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(fk => new { fk.CustomerID })
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete


            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { RestaurantId = 1, BusinessNum = "bhre5999", BusinessUrl = "www.blah.com", CityName = "Vancouver", Email = "hotters@hooters.com", Province = "BC", RestaurantName = "Hooters", StreetAddress = "1 Main Street", OpeningHour = 9, ClosingHour = 22 },
                 new Restaurant { RestaurantId = 2, BusinessNum = "bhre5984", BusinessUrl = "www.wendys.com", CityName = "Vancouver", Email = "wendy@wendys.com", Province = "BC", RestaurantName = "Wendys", StreetAddress = "10 Main Street", OpeningHour = 9, ClosingHour = 22 });
            modelBuilder.Entity<RestaurantTable>().HasData(
                new RestaurantTable { RestaurantID = 1, TableCapacity = 4, RestaurantTableID = 1 },
                                new RestaurantTable { RestaurantID = 1, TableCapacity = 6, RestaurantTableID = 2 },
                                 new RestaurantTable { RestaurantID = 2, TableCapacity = 4, RestaurantTableID = 3 },
                           new RestaurantTable { RestaurantID = 2, TableCapacity = 4, RestaurantTableID = 4 }
                                );
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerID = 1, bookingName = "Eugene", PhoneNum = "2525258987" });
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { ReservationID = 1, CustomerID = 1, RestaurantTableID = 1, ReservationStart = new DateTime(2022, 3, 16, 12, 0, 0), ReservationEnd = new DateTime(2022, 3, 16, 13, 0, 0) });


        }
    }
}
