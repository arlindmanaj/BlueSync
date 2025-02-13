using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using BlueSync.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlueSync.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceGroup> DeviceGroups { get; set; }
        public DbSet<AudioSession> AudioSessions { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            // DeviceGroup has many Devices, but Device does not hold a foreign key directly.
            modelBuilder.Entity<DeviceGroup>()
                .HasMany(g => g.Devices)
                .WithOne()  // No navigation property in Device
                .HasForeignKey("DeviceGroupId")  // Define the FK at the DB level but NOT in the model
                .OnDelete(DeleteBehavior.Cascade);

            // AudioSession references DeviceGroup (one-to-one relationship)
            modelBuilder.Entity<AudioSession>()
                .HasOne(a => a.Group)
                .WithMany()
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        

        }

    }
}
