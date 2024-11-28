using ProjectMVC.DAL.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ProjectMVC.DAL.Repository.Interfaces
{
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
}
