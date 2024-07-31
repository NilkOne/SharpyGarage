using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories
{
    public interface IVéhiculeRepository
    {
        Task<Véhicule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
