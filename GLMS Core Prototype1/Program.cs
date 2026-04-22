using GLMS_Core_Prototype.Data;
using GLMS_Core_Prototype.Services;
using GLMS_Core_Prototype.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace GLMS_Core_Prototype
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));
            builder.Services.AddHttpClient<CurrencyService>();
            builder.Services.AddScoped<FileService>();
            builder.Services.AddScoped<IModelValidator<GLMS_Core_Prototype.Models.Client>, ClientValidator>();
            builder.Services.AddScoped<IModelValidator<GLMS_Core_Prototype.Models.Contract>, ContractValidator>();
            builder.Services.AddScoped<ServiceRequestValidator>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (db.Database.GetAppliedMigrations().Any())
                {
                    db.Database.Migrate();
                }
                else
                {
                    db.Database.EnsureCreated();
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
