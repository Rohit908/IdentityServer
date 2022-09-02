using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4Demo
{
    public static class IdentityServerConfiguration
    {

        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "custom.scope",
                UserClaims =
                {
                    "permission"
                }
            }
        };

        public static IEnumerable<ApiResource> GetApis() => 
            new List<ApiResource> 
            { 
                new ApiResource("ApiOne")
            };

        public static IEnumerable<ApiScope> GetApiScopes() => 
            new List<ApiScope> 
            { 
                new ApiScope("ApiOne") 
            };

        public static IEnumerable<Client> GetClients() => 
            new List<Client> 
            { 
                new Client() { 
                    ClientId = "client_id",
                    ClientSecrets = {new Secret("client_secret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {
                            "ApiOne",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "custom.scope"},
                     RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }
}