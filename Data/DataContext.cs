using Microsoft.EntityFrameworkCore;
using EmLock.API.Models;

namespace EmLock.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<EMIPlan> EMIPlans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<EmiSchedule> EmiSchedules { get; set; }
        public DbSet<EmiLog> EmiLogs { get; set; }
        public DbSet<DeviceActionLog> DeviceActionLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
