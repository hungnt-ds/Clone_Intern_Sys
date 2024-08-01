using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories;

public class UserRepository : BaseRepository<AspNetUser>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AspNetUser>> GetUsersByHoVaTenAsync(string hoVaTen)
    {
        var searchTerm = hoVaTen.Trim().ToLower();
        var users = await _dbContext.Users
                               .Where(u => u.HoVaTen.ToLower().Contains(searchTerm))
                               .ToListAsync();
        return users;
    }

    public async Task UpdateAsync(AspNetUser user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AspNetUser> GetUserByRefreshTokenAsync(string resetToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.ResetToken == resetToken);
    }

    public async Task UpdateUserAsync(AspNetUser user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> GetUserNameByIdAsync(object id)
    {
        var user = await _dbContext.Set<AspNetUser>().FindAsync(id);
        return user.HoVaTen;
    }

    public async Task<Dictionary<string, string>> GetFullNamesByIdsAsync(IEnumerable<string> userIds)
    {
        // Ensure that the list of userIds is not empty
        if (!userIds.Any())
        {
            return new Dictionary<string, string>();
        }

        // Query the database for user names
        var users = await _dbContext.Users
            .Where(user => userIds.Contains(user.Id))
            .Select(user => new { user.Id, user.HoVaTen })
            .ToListAsync();

        // Convert the list of users to a dictionary
        return users.ToDictionary(u => u.Id, u => u.HoVaTen);
    }
}