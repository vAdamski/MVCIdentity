using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace DuendeIdentityServerwithASP.NETCoreIdentity1;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource(
                name: "roles",
                userClaims: new[] {JwtClaimTypes.Role})
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // interactive ASP.NET Core MVC client
            new Client
            {
                ClientId = "mvc",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
            
                // where to redirect to after login
                RedirectUris = { "https://localhost:6001/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles"
                }
            }
        };
}