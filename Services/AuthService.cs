using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectMVC.Models;
using ProjectMVC.Models;
using ProjectMVC.Repositories;

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

				return Result.Failure(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
			}

			await _authRepository.AddClaimAsync(user, new Claim("Name", user.Name));
			await _authRepository.SignInAsync(user, false);

			return Result.Success($"Registration successful\n\n Welcome, {user.Name}!");
		}

		public async Task<Result> LoginUserAsync(LoginDto loginDto)
		{
			var user = await _authRepository.FindByEmailAsync(loginDto.Email);
			if (user == null || !await _authRepository.CheckPasswordAsync(user, loginDto.Password))
			{
				return Result.Failure("Invalid email or password");
			}

			var userClaims = await _authRepository.GetUserClaimsAsync(user);
			if(!userClaims.Any(c => c.Type == "Name"))
			{
				await _authRepository.AddClaimAsync(user, new Claim("Name", user.Name ?? "Unknown User"));
			}

			await _authRepository.SignInAsync(user, false);

			return Result.Success($"Welcome {user.Name}");
		}

		public async Task<Result> LogoutUserAsync()
		{
			await _authRepository.SignOutAsync();
			return Result.Success("You have been successfully logged out");
		}
	}
}
