using System.Security.Claims;
using IdentityModel;
using DuendeIdentityServerwithASP.NETCoreIdentity1.Data;
using DuendeIdentityServerwithASP.NETCoreIdentity1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DuendeIdentityServerwithASP.NETCoreIdentity1;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var adminRole = roleMgr.FindByNameAsync("admin").Result;
            if (adminRole == null)
            {
                adminRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };

                var result = roleMgr.CreateAsync(adminRole).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            
            var managerRole = roleMgr.FindByNameAsync("manager").Result;
            if (managerRole == null)
            {
                managerRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "manager",
                    NormalizedName = "MANAGER"
                };

                var result = roleMgr.CreateAsync(managerRole).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            
            var userRole = roleMgr.FindByNameAsync("user").Result;
            if (userRole == null)
            {
                userRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "user",
                    NormalizedName = "USER"
                };

                var result = roleMgr.CreateAsync(userRole).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            
            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var aliceRoles = userMgr.GetRolesAsync(alice).Result;
            if (!aliceRoles.Contains("admin"))
            {
                var result = userMgr.AddToRoleAsync(alice, "admin").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
            
            var bobRoles = userMgr.GetRolesAsync(bob).Result;
            if (!bobRoles.Contains("user"))
            {
                var result = userMgr.AddToRoleAsync(bob, "user").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }
}