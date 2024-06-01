using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalksAuthDbContext:IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "86e51e9d-4c61-47d1-9eb4-b47153a36f3a";
            var writerRoleId = "3a229f48-337b-474c-b667-ae59fd292d47";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                     Id=readerRoleId,
                      ConcurrencyStamp=readerRoleId,
                       Name="Reader",
                       NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                     Id=writerRoleId,
                      ConcurrencyStamp=writerRoleId,
                       Name="Writer",
                       NormalizedName="Writer".ToUpper()
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
