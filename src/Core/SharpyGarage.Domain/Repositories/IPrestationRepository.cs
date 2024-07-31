using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories
{
    public interface IPrestationRepository
    {
        Task<Prestation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
