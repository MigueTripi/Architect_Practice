using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SelfResearch.Financial.API.Core.Data;

namespace SelfResearch.Financial.API.Feature.Propagate;

[ExcludeFromCodeCoverage]
public class PropagatedUserRepository : IPropagatedEntityRepository<PropagatedUser>
{

    private readonly AppDbContext _context;

    public PropagatedUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PropagatedUser> CreateAsync(PropagatedUser entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<PropagatedUser?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
