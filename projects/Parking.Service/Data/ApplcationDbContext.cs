using Microsoft.EntityFrameworkCore;
using Parking.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.API.Data
{
    public class ApplcationDbContext : DbContext
    {
        public ApplcationDbContext(DbContextOptions<ApplcationDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Park>()
        //        .HasMany(p => p.ParkingSpots)
        //        .WithOne(ps => ps.Park)
        //        .HasForeignKey(ps => ps.ParkingId)
        //        .OnDelete(DeleteBehavior.Cascade);


        //}

        public DbSet<Park> Parks { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
    }
}
