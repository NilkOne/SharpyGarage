using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories;

public interface IGarageRepository
{
    Task<Garage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
