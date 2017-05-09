using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pare.Sample.Domain;
using Microsoft.EntityFrameworkCore;
using pare.EntityFrameworkCore;
using System.Reflection;
using pare.EntityFrameworkCore.Seed;
using pare.Sample.Seeding;

namespace pare.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();


            //note that we are store the migrations in the app space, could have used "pare.Sample"
            ConfigureDatabase<SampleContext>(services, o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("pare.Sample")));

            services.AddSeeding<SeedSampleContext>()
                .AddSingleton<ISeedData, SeedSampleContext>(provider => provider.GetService<SeedSampleContext>());
        }

        private void ConfigureDatabase<TContext>(IServiceCollection services, Action<DbContextOptionsBuilder> action) where TContext : DbContext, IDbContext
        {
            services.AddDbContext<TContext>(action);
            services.AddScoped<IDbContext, TContext>(provider => provider.GetService<TContext>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexts = serviceScope.ServiceProvider.GetServices<IDbContext>();
                foreach (var context in contexts)
                    ((DbContext)context).Database.Migrate();

                var seeding = serviceScope.ServiceProvider.GetServices<ISeedData>();
                foreach (var item in seeding)
                {
                    item.SeedEnums(env.EnvironmentName);
                    item.Seed(env.EnvironmentName);
                }
            }

            app.UseMvc();
        }
    }
}
