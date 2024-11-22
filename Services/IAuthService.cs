using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectMVC.Models;

public interface IAuthService
{
    Task<(bool Succeeded, string[] Errors)> RegisterUserAsync(UserDto userDto);
    Task<(bool Succeeded, string[] Errors)> LoginUserAsync(LoginDto loginDto);
    Task LogoutUserAsync();
}

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<(bool Succeeded, string[] Errors)> RegisterUserAsync(UserDto userDto)
    {
        var user = new ApplicationUser
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            Name = userDto.Name
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddClaimAsync(user, new Claim("Name", user.Name));
            await _signInManager.SignInAsync(user, isPersistent: false);
            return (true, Array.Empty<string>());
        }

        return (false, result.Errors.Select(e => e.Description).ToArray());
    }

    public async Task<(bool Succeeded, string[] Errors)> LoginUserAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return (false, new[] { "Invalid email or password" });
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            if (!userClaims.Any(c => c.Type == "Name"))
            {
                await _userManager.AddClaimAsync(user, new Claim("Name", user.Name));
            }
            return (true, Array.Empty<string>());
        }

        return (false, new[] { "Invalid email or password" });
    }

    public async Task LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
