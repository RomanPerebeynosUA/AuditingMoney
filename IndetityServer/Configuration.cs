using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IndetityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
             new List<IdentityResource>
             {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope ",
                    UserClaims = 
                    {
                        "rc.grandma"
                    }

                }
             };
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("SpendingDiary"),
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {

                    // Потрібна інформація для індетнифікації клієнта 
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256())}, 

                    // Отримання токену 
                    AllowedGrantTypes = GrantTypes.ClientCredentials, 

                    // access to some API
                    AllowedScopes = { "SpendingDiary"}

                },
                      new Client
                {
                    // Потрібна інформація для індетнифікації клієнта 
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256())}, 

                    // Отримання токену 
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44387/signin-oidc" },

                   

                    // access to some API
                    AllowedScopes = {"SpendingDiary",  
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope",
                     },
                    RequireConsent = false,
                   
                     // puts all the claims in the id token     
                  //  AlwaysIncludeUserClaimsInIdToken = true,


                }
            };
        public static List<TestUser> GetTestUsers()
        {
            return new[] {
            new TestUser
            {
                Username = "admin",
                Password = "secret",
                Claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    // Also tried this one, but to no avail:
                  // new Claim("AspNet.Identity.SecurityStamp", Guid.NewGuid().ToString())
                },
            }
        }.ToList();
        }
    }
}
