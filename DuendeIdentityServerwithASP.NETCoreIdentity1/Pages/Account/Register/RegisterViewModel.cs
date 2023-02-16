using System.ComponentModel.DataAnnotations;

namespace DuendeIdentityServerwithASP.NETCoreIdentity1.Pages.Account.Register;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    
    public string Button { get; set; }
    public string ReturnUrl { get; set; }
}