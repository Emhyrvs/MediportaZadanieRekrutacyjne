using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using Microsoft.EntityFrameworkCore;



namespace MediportaZadanieRekrutacyjne.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<TagItem> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagItem>()
                .Property(t => t.Share)
                .HasColumnType("decimal(18,2)"); // Precyzja i skala dla właściwości "Share"
        }
        
    }
}