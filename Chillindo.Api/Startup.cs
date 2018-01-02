using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Chillindo.Core.Data;
using Chillindo.Data.Repositories;
using Chillindo.Data;
using Microsoft.EntityFrameworkCore;
using Chillindo.Data.Seeds;

namespace Chillindo.Api
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
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddDbContext<ChillindoContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:ChillindoDatabaseConnection"],
                b=>b.MigrationsAssembly("Chillindo.Api")));
            if (Env.IsEnvironment("Test"))
            {
                services.AddDbContext<ChillindoContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "Chillindo"));
            }
            else
            {
                services.AddDbContext<ChillindoContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:ChillindoDatabaseConnection"],
                    b => b.MigrationsAssembly("Chillindo.Api")));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ChillindoContext db)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            db.EnsureSeedData();
        }
    }
}
