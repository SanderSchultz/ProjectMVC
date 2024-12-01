using ProjectMVC.Models;
using ProjectMVC.DAL.Entities;
using ProjectMVC.DTO;

namespace ProjectMVC.DAL.Repository.Interfaces
{
	public interface IProfileRepository
	{
		Task UpdateProfile(ApplicationUser user);
		Task<ApplicationUser?> FindUserByEmailAsync(string userId);
		Task<UserDto> GetUserAsync(string userId);
	}
}
