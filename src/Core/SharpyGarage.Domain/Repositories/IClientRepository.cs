using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Domain.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
