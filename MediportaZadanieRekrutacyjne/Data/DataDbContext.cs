using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using Microsoft.EntityFrameworkCore;



namespace MediportaZadanieRekrutacyjne.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<TagItem> Tags { get; set; }
    }
}