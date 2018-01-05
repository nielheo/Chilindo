using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Chilindo.Core.Data;
using Chilindo.Data.Repositories;
using Chilindo.Data;
using Microsoft.Extensions.Logging;
using Chilindo.Data.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Chilindo_React
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
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddDbContext<ChilindoContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:ChilindoDatabaseConnection"],
                b => b.MigrationsAssembly("Chilindo.Api")));
            if (Env.IsEnvironment("Test"))
            {
                services.AddDbContext<ChilindoContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "Chilindo"));
            }
            else
            {
                services.AddDbContext<ChilindoContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:ChilindoDatabaseConnection"],
                    b => b.MigrationsAssembly("Chilindo.Api")));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, ChilindoContext db)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });


            db.EnsureSeedData();
        }
    }
}
