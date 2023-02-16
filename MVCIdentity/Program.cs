using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("admin", policy =>
    {
        policy.RequireClaim("role", "admin");
        policy.RequireAuthenticatedUser();
    });
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.MapAll();
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role
        };

        options.SaveTokens = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute()
        .RequireAuthorization();
});

app.Run();