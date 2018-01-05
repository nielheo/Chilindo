using Chilindo.Core.Data;
using Chilindo.Data;
using Chilindo.Data.Repositories;
using Chilindo.Data.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chilindo.Api
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
            }

            app.UseMvc();

            db.EnsureSeedData();
        }
    }
}