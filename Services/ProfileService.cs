using ProjectMVC.Models;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.Services.Interfaces;

public class ProfileService : IProfileService
{
    private readonly IProfileService _profileService;

    // public ProfileService(IProfileService profileService)
    // {
    //     _profileService = profileService;
    // }

    public async Task<Result> GetProfile(string postId, string userId)
    {

		if(string.IsNullOrEmpty(userId))
		{
			return Result.Failure("You need to be signed in to see your profile");
		}

		return Result.Success("Liked post successfully");

    }
}
