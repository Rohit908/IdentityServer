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
            new IdentityResources.Profile()
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
                    ClientId = "client1",
                    ClientSecrets = {new Secret("secret1".ToSha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        "ApiOne"
                    }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:44367/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:44367/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ApiOne",
                    },
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }
}