using System.Security.Claims;
using ProjectMVC.Models;
using ProjectMVC.DTO;
using ProjectMVC.DAL.Entities;
using ProjectMVC.Services.Interfaces;
using ProjectMVC.DAL.Repository.Interfaces;

namespace ProjectMVC.Services
{
	public class AuthService : IAuthService
	{
		private readonly IAuthRepository _authRepository;

		public AuthService(IAuthRepository authRepository)
		{
			_authRepository = authRepository;
		}

		public async Task<Result> RegisterUserAsync(UserDto userDto)
		{

			var existingUser = await _authRepository.FindByEmailAsync(userDto.Email);
			if(existingUser != null)
			{
				return Result.Failure("Email already in use");
			}

			if(userDto.Password != userDto.ConfirmPassword)
			{
				return Result.Failure("Passwords do not match");
			}

			var user = new ApplicationUser
			{
				UserName = userDto.Email,
				Email = userDto.Email,
				Name = userDto.Name
			};

			var result = await _authRepository.CreateUserAsync(user, userDto.Password);

			if (!result.Succeeded)
			{
				return Result.Failure(string.Join(Environment.NewLine, result.Errors.Select(error => error.Description)));
			}

			await _authRepository.AddClaimAsync(user, new Claim("Name", user.Name ?? string.Empty));
			await _authRepository.AddClaimAsync(user, new Claim("ProfilePicture", user.ProfilePicture ?? string.Empty));
			await _authRepository.AddClaimAsync(user, new Claim("Email", user.Email ?? string.Empty));

			await _authRepository.SignInAsync(user, false);

			return Result.Success($"Welcome, {user.Name}!");
		}

		public async Task<Result> LoginUserAsync(LoginDto loginDto)
		{
			var user = await _authRepository.FindByEmailAsync(loginDto.Email);
			if (user == null || !await _authRepository.CheckPasswordAsync(user, loginDto.Password))
			{
				return Result.Failure("Invalid email or password");
			}

			var userClaims = await _authRepository.GetUserClaimsAsync(user);

			if(!userClaims.Any(claims => claims.Type == "Name"))
			{
				await _authRepository.AddClaimAsync(user, new Claim("Name", user.Name ?? string.Empty));
			}

			if (!userClaims.Any(claims => claims.Type == "ProfilePicture"))
			{
				await _authRepository.AddClaimAsync(user, new Claim("ProfilePicture", user.ProfilePicture ?? string.Empty));
			}

			if (!userClaims.Any(claims => claims.Type == "Email"))
			{
				await _authRepository.AddClaimAsync(user, new Claim("Email", user.Email ?? string.Empty));
			}

			await _authRepository.SignInAsync(user, false);

			return Result.Success($"Welcome, {user.Name}!");
		}

		public async Task<Result> LogoutUserAsync()
		{
			await _authRepository.SignOutAsync();
			return Result.Success("You have been successfully logged out");
		}
	}
}
