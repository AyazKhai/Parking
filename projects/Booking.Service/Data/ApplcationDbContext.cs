using Booking.Service.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Data
{
    public class ApplcationDbContext : DbContext
    {
        public ApplcationDbContext(DbContextOptions<ApplcationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ParkEntity> Parks { get; set; }
        public DbSet<ParkingSpotEntity> ParkingSpots { get; set; }
    }
}
