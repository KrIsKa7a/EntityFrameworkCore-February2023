namespace PetStore.Web
{
    using System.Reflection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Common.Repos;
    using Data.Repositories;
    using Services.Data;
    using Services.Mapping;
    using ViewModels;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplication app = ConfigureServices(args);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static WebApplication ConfigureServices(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string connectionString = builder
                .Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder
                .Services
                .AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString));
            builder
                .Services
                .AddDatabaseDeveloperPageExceptionFilter();

            builder
                .Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 5;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder
                .Services
                .AddControllersWithViews();

            //builder
            //    .Services
            //    .AddAutoMapper(typeof(Program));
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Repositories
            builder
                .Services
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            builder
                .Services
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));

            // Services
            builder
                .Services
                .AddTransient<ICategoryService, CategoryService>();

            WebApplication app = builder.Build();

            return app;
        }
    }
}