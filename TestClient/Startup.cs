using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestClient
{
    public class Startup
    {
     
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
              .AddCookie("Cookie")
              .AddOpenIdConnect("oidc", config =>
              {
                  config.Authority = "https://localhost:44357/";
                  config.ClientId = "client_id_mvc";
                  config.ClientSecret = "client_secret_mvc";
                  config.SaveTokens = true;

                  config.ResponseType = "code";

                  //configure cookie claim mapping
                  //config.ClaimActions.DeleteClaim("amr");
                  //config.ClaimActions.MapUniqueJsonKey("Some.Grandma", "rc.grandma");

                  ////configure scope
                  //config.Scope.Clear();
                  //config.Scope.Add("openid");
                  //config.Scope.Add("rs.scope");

                  //two trips to load claims in the cookie
                  //but the id token is smaller !
                  //config.GetClaimsFromUserInfoEndpoint = true;
              });

            services.AddHttpClient();
            services.AddControllersWithViews();
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
