using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using DuendeIdentityServerwithASP.NETCoreIdentity1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DuendeIdentityServerwithASP.NETCoreIdentity1.Pages.Account.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityServerInteractionService _interaction;

    
    [BindProperty] 
    public RegisterViewModel RegisterInput { get; set; }

    public Index(IIdentityServerInteractionService interaction, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _interaction = interaction;
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl)
    {
        await BuildModelAsync(returnUrl);

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var context = await _interaction.GetAuthorizationContextAsync(RegisterInput.ReturnUrl);

        // the user clicked the "cancel" button
        if (RegisterInput.Button != "register")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(RegisterInput.ReturnUrl);
                }

                return Redirect(RegisterInput.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if (RegisterInput.Password != RegisterInput.ConfirmPassword)
        {
            
        }
        
        var userExists = await _userManager.FindByNameAsync(RegisterInput.Username);

        ApplicationUser user = new ApplicationUser()
        {
            Email = RegisterInput.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = RegisterInput.Username,
            EmailConfirmed=true,
            PhoneNumberConfirmed=true,
        };
        
        var response = await _userManager.CreateAsync(user, RegisterInput.Password);

        await _userManager.AddToRoleAsync(user, "user");

        if (string.IsNullOrWhiteSpace(RegisterInput.ReturnUrl))
        {
            return Redirect("~/");
        }

        return Redirect(RegisterInput.ReturnUrl);
    }
    
    private async Task BuildModelAsync(string returnUrl)
    {
        RegisterInput = new RegisterViewModel()
        {
            ReturnUrl = returnUrl
        };
    }
}
