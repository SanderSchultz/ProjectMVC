using ProjectMVC.Models;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DTO;
using System.Security.Claims;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

public class ProfileService : IProfileService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IProfileRepository _profileRepository;

    public ProfileService(IHttpContextAccessor httpContextAccessor, IProfileRepository profileRepository)
    {
        _httpContextAccessor = httpContextAccessor;
		_profileRepository = profileRepository;
    }

	public async Task<UserDto> GetUserAsync()
	{
		var user = _httpContextAccessor.HttpContext?.User;
		var email = user?.FindFirst(ClaimTypes.Email)?.Value;

		var result = await _profileRepository.GetUserAsync(email!);

		return result;
	}

	public async Task<Result> UpdateProfile(UserDto dto)
	{

		var hasher = new PasswordHasher<ApplicationUser>();
		var user = _httpContextAccessor.HttpContext?.User;
		var email = user?.FindFirst(ClaimTypes.Email)?.Value;

		if(user?.Identity?.IsAuthenticated == false)
		{
			return Result.Failure("You need to be signed in to update your account");
		}

		var applicationUser = await _profileRepository.FindUserByEmailAsync(email!);

		if (!string.IsNullOrEmpty(dto.OldPassword) || !string.IsNullOrEmpty(dto.Password) || !string.IsNullOrEmpty(dto.ConfirmPassword))
		{
			if (string.IsNullOrEmpty(dto.OldPassword) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.ConfirmPassword))
			{
				return Result.Failure("You need to fill out all password fields to update the password.");

			} else {

				if(dto.Password == dto.ConfirmPassword)
				{
					var verification = hasher.VerifyHashedPassword(applicationUser!, applicationUser!.PasswordHash!, dto.OldPassword);
					if(verification == PasswordVerificationResult.Failed)
					{
						return Result.Failure("Wrong old password");
					}
					applicationUser.PasswordHash = hasher.HashPassword(applicationUser, dto.Password);
				} else {
					return Result.Failure("The new passwords do not match");
				}

			}
		}

		if(!string.IsNullOrEmpty(dto.Name))
		{
			applicationUser!.Name = dto.Name;
		}

		var image = string.Empty;

		if (dto.ProfilePictureIForm != null && dto.ProfilePictureIForm.Length > 0)
		{

			if(!string.IsNullOrEmpty(applicationUser!.ProfilePicture))
			{

				var deletefilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", applicationUser.ProfilePicture.TrimStart('/'));

				if (File.Exists(deletefilePath))
				{
					File.Delete(deletefilePath);
				}


			}

			var fileName = Guid.NewGuid() + Path.GetExtension(dto.ProfilePictureIForm.FileName);
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

			Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await dto.ProfilePictureIForm.CopyToAsync(stream);
			}

			applicationUser.ProfilePicture = $"/images/{fileName}";

		}

		await _profileRepository.UpdateProfile(applicationUser!);

		return Result.Success("Profile updated");

	}
}
