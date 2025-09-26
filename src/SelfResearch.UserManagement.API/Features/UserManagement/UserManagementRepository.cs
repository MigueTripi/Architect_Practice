using System.Diagnostics.CodeAnalysis;
using SelfResearch.UserManagement.API.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

[ExcludeFromCodeCoverage]
public class UserManagementRepository : IUserManagementRepository
{
    private readonly AppDbContext _dbContext;

    public UserManagementRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<User> CreateUserAsync(User user)
    {
        await this._dbContext.AddAsync(user);
        await this._dbContext.SaveChangesAsync();
        return user;
    }

    /// <inheritdoc/>
    public async Task<List<User>> GetPagedUsersAsync(int skip, int take)
    {
        return await _dbContext.Users.Skip(skip).Take(take).ToListAsync();
    }

    /// <inheritdoc/>
    public Task<User?> GetUserAsync(int id) => _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

    /// <inheritdoc/>
    public async Task UpdateUserAsync()
    {
        await this._dbContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteUserAsync(int id)
    {
        var user = await this._dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user != null)
        {
            this._dbContext.Users.Remove(user);
        }
        await this._dbContext.SaveChangesAsync();
    }
}
