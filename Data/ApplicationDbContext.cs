using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTDAttendancePortal_backend.Models.Entities;

namespace TTDAttendancePortal_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Attendance> Attendance { get; set; }


    }
}


