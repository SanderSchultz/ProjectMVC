using ProjectMVC.Models;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(UserDto dto);
    Task<Result> LoginUserAsync(LoginDto dto);
    Task<Result> LogoutUserAsync();
}
