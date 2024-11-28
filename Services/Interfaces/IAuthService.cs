using ProjectMVC.Models;
using ProjectMVC.DTO;

namespace ProjectMVC.Services.Interfaces
{
	public interface IAuthService
	{
		Task<Result> RegisterUserAsync(UserDto dto);
		Task<Result> LoginUserAsync(LoginDto dto);
		Task<Result> LogoutUserAsync();
	}
}
