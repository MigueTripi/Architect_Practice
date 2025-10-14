using System;
using SelfResearch.Financial.API.Core.Data;

namespace SelfResearch.Financial.API.Feature.Propagate;

public class PropagatedUserService : IPropagatedEntityService<PropagatedUser>
{
    private readonly IPropagatedEntityRepository<PropagatedUser> _repository;

    public PropagatedUserService(IPropagatedEntityRepository<PropagatedUser> repository)
    {
        _repository = repository;
    }

    public async Task<PropagatedUser?> CreateAsync(PropagatedUser entity)
    {
        var user = await _repository.GetByIdAsync(entity.Id);
        if (user != null)
        {
            return user;
        }
        return await _repository.CreateAsync(entity);
    }

    public async Task<PropagatedUser?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
