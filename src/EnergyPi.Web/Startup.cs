using EnergyPi.Web.Builders;
using EnergyPi.Web.Data;
using EnergyPi.Web.DataServices;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EnergyPi.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthenticationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("AuthenticationConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationDbContext>();

            services.AddDbContext<DataDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DataConnection")));

            services = AddCustomServices(services);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        private IServiceCollection AddCustomServices(IServiceCollection services)
        {
            // repositories
            services.AddTransient<IRepository<EnergyLogs>, DataRepository<EnergyLogs>>();
            services.AddTransient<IRepository<WeatherLogs>, DataRepository<WeatherLogs>>();

            // data services
            services.AddTransient<IEnergyLogsDataService, EnergyLogsDataService>();
            services.AddTransient<IWeatherLogsDataService, WeatherLogsDataService>();

            // builders
            services.AddTransient<IDashboardViewModelBuilder, DashboardViewModelBuilder>();
            services.AddTransient<IHistoryViewModelBuilder, HistoryViewModelBuilder>();

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
            });
        }

    }
}
