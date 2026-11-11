using AidHopePlus.Data;
using AidHopePlus.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AidHopePlus
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ========== AZURE SQL CONNECTION ==========
            // Replace with your real Azure SQL connection string
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

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register services
            builder.Services.AddSingleton<DatabaseService>();

            // Register pages
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<Views.DashboardPage>();
            builder.Services.AddTransient<Views.BeneficiariesPage>();
            builder.Services.AddTransient<Views.AwarenessEventsPage>();
            builder.Services.AddTransient<Views.SupportSessionsPage>();
            builder.Services.AddTransient<Views.AboutPage>();
            builder.Services.AddTransient<Views.AttendancePage>();
            builder.Services.AddTransient<Views.ReportsPage>();
            builder.Services.AddTransient<Views.UserManagementPage>();




#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
