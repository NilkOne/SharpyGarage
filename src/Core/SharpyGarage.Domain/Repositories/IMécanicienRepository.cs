using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories
{
    public interface IMécanicienRepository
    {
        Task<Mécanicien?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
