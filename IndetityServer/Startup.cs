using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IndetityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IndetityServer
{
    public class Startup
    {
        private readonly IConfiguration _config; 
        public Startup(IConfiguration config)
        {
            _config = config;
        }
       
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlServer("ConnectionString");
                //config.UseInMemoryDatabase("Memory");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>().
                AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            string assembly = typeof(Startup).Assembly.GetName().ToString();
            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddTestUsers(Configuration.GetTestUsers())
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                         sql => sql.MigrationsAssembly(assembly));
                 })
                  .AddOperationalStore(options =>
                  {
                      options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                          sql => sql.MigrationsAssembly(assembly));
                  })
            //.AddInMemoryApiResources(Configuration.GetApis()) //add apis
                //.AddInMemoryClients(Configuration.GetClients()) // add clients
               
                //.AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddDeveloperSigningCredential(); // create server key 

            services.AddControllersWithViews();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
