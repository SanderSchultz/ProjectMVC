using ProjectMVC.Models;
using ProjectMVC.DTO;
using ProjectMVC.DAL.Entities;

namespace ProjectMVC.Services.Interfaces
{
	public interface IProfileService
	{
		Task<Result> UpdateProfile(UserDto dto);
		Task<UserDto> GetUserAsync();
	}
}
