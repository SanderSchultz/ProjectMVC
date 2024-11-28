using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;

namespace ProjectMVC.DAL.Repository
{
	public class AuthRepository : IAuthRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<ApplicationUser?> FindByEmailAsync(string email) =>
			await _userManager.FindByEmailAsync(email);

		public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password) =>
			await _userManager.CreateAsync(user, password);

		public async Task AddClaimAsync(ApplicationUser user, Claim claim) =>
			await _userManager.AddClaimAsync(user, claim);

		public async Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user) =>
			await _userManager.GetClaimsAsync(user);

		public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
			await _userManager.CheckPasswordAsync(user, password);

		public async Task SignInAsync(ApplicationUser user, bool isPersistent) =>
			await _signInManager.SignInAsync(user, isPersistent);

		public async Task SignOutAsync() =>
			await _signInManager.SignOutAsync();
	}
}
