using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories
{
    public interface IOrdreRéparationRepository
    {
        Task<OrdreRéparation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Add(OrdreRéparation rendezVous);
    }
}
