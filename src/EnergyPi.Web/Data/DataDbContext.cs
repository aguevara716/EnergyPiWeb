using EnergyPi.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnergyPi.Web.Data
{
    public partial class DataDbContext : DbContext
    {
        public DataDbContext()
        {
        }

        public DataDbContext(DbContextOptions<DataDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EnergyLogs> EnergyLogs { get; set; }
        public virtual DbSet<WeatherLogs> WeatherLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=10.0.0.39;user id=energypi;password=energy;database=energypi", x => x.ServerVersion("10.3.17-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnergyLogs>(entity =>
            {
                entity.HasKey(e => e.EnergyLogId)
                    .HasName("PRIMARY");

                entity.Property(e => e.EnergyLogId).HasColumnType("int(11)");

                entity.Property(e => e.Delta).HasColumnType("decimal(12,2)");

                entity.Property(e => e.PowerDraw).HasColumnType("decimal(12,2)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.TotalConsumption).HasColumnType("decimal(12,2)");
            });

            modelBuilder.Entity<WeatherLogs>(entity =>
            {
                entity.HasKey(e => e.WeatherLogId)
                    .HasName("PRIMARY");

                entity.Property(e => e.WeatherLogId).HasColumnType("int(11)");

                entity.Property(e => e.Humidity).HasColumnType("decimal(3,2)");

                entity.Property(e => e.TemperatureFahrenheit).HasColumnType("decimal(5,2)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
