using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Installers
{
    public class DatabaseInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            //------------------------ 
            // builder.Configuration.GetConnectionString เอาไว้อ่านค่าใน appsettings
            // Services มาจาก WebApplicationBuilder
            var connectionString = builder.Configuration.GetConnectionString("ProjectSummerContext");
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString)
            );
            //------------------------
        }
    }
}
