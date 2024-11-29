using ProjectMVC.Models;

namespace ProjectMVC.Services.Interfaces
{
	public interface IProfileService
	{
		Task<Result> GetProfile(string userId, string actual_userId);
	}
}
