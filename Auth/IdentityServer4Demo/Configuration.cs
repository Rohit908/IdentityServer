using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4Demo
{
    public static class Configuration
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
                    "Permission"
                }
            }
        };
        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource> { 
            new ApiResource("ApiOne"), 
            new ApiResource("ApiTwo")
        };

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope> { new ApiScope("ApiOne"), new ApiScope("ApiTwo") };

        public static IEnumerable<Client> GetClients() => new List<Client> { 
            new Client() { 
            ClientId = "client_id",
            ClientSecrets = {new Secret("client_secret".ToSha256())},
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = {"ApiOne"}},

            new Client() {
            ClientId = "client_id_mvc",
            ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = {"https://localhost:44339/signin-oidc" },
            AllowedScopes = {
                    "ApiOne", 
                    "ApiTwo", 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "custom.scope"},
            RequireConsent = false,
            AlwaysIncludeUserClaimsInIdToken = true
            }
        };
    }
}