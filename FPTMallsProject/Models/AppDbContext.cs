using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Brand> Brand { get; set; }

        public DbSet<Shop> Shop { get; set; }

        public DbSet<BrandIntroduction> BrandIntroduction { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Show> Shows { get; set; }

        public DbSet<Booking> Bookings { get; set; }
    }
}
