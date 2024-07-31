using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories
{
    public interface IRendezVousRepository
    {
        Task<RendezVous?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Add(RendezVous rendezVous);
    }
}
