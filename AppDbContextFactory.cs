using Core_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Core_WebAPI
{
    public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use Railway public MySQL URL
            optionsBuilder.UseMySql(
                 "Server=nozomi.proxy.rlwy.net;Port=52285;Database=railway;User=root;Password=dPvHpaNdPkBBzudVBHgicOsIhdwUmDrm;SslMode=Preferred;",
                 new MySqlServerVersion(new Version(8, 0, 33)),
                 mySqlOptions =>
                 {
                     mySqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(10),
                         errorNumbersToAdd: null
                     );
                 }
             );


            return new AppDbContext(optionsBuilder.Options);
        }

    }
}
