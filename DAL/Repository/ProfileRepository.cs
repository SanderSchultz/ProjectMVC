using ProjectMVC.DAL.Repository.Interfaces;
using ProjectMVC.DAL;
using ProjectMVC.DTO;
using ProjectMVC.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

	public async Task<ApplicationUser?> FindUserByEmailAsync(string email)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.Email == email);

		return user;
	}

	public async Task<UserDto> GetUserAsync(string email)
	{

		var user = await _context.Users
			.AsNoTracking()
			.Where(u => u.Email == email)
			.Select(u => new UserDto
			{
				Id = u.Id,
				Name = u.Name,
				Email = u.Email!,
				ProfilePicture = u.ProfilePicture
			})
			.FirstOrDefaultAsync();

		return user!;
	}

    public async Task UpdateProfile(ApplicationUser user)
    {
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
    }

}
