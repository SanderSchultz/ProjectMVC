using ProjectMVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public interface IAuthRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task AddClaimAsync(ApplicationUser user, Claim claim);
    Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task SignInAsync(ApplicationUser user, bool isPersistent);
    Task SignOutAsync();
}
