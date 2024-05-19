using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NZWalksDbContext:DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        DbSet<Difficulty> Difficulties { get; set; }

        DbSet<Walk> Walks { get; set; }

        DbSet<Region> Regions { get; set; }
    }
}
