using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        // ✅ The return type here MUST be ApplicationDbContext, NOT void
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var cs = "server=localhost;port=3306;database=WhiteLagoon;user=root;password=Zerominde1.";

            optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}