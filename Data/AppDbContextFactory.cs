using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AidHopePlus.Data
{
    /// <summary>
    /// This class is only used by EF Core tools (Add-Migration / Update-Database)
    /// so they know how to create the DbContext at design time.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // IMPORTANT: Put the same Azure SQL connection string here
            // that you use in MauiProgram.cs
            string connectionString =
                "Server=tcp:st10103378.database.windows.net,1433;" +
                "Initial Catalog=AidHopePlusDB;" +
                "Persist Security Info=False;" +
                "User ID=st10103378;" +
                "Password=2025#Lis;" +
                "MultipleActiveResultSets=False;" +
                "Encrypt=True;" +
                "TrustServerCertificate=False;" +
                "Connection Timeout=30;";

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
