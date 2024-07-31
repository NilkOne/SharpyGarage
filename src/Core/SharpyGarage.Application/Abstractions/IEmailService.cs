using Sharpy.SharpyGarage.Domain.Entities;

namespace Sharpy.SharpyGarage.Application.Abstractions;

public interface IEmailService
{
    Task SendDemandeRendezVousEmailAsync(RendezVous rendezVous, CancellationToken cancellationToken = default);
    Task SendAcceptationRendezVousEmailAsync(RendezVous rendezVous, CancellationToken cancellationToken = default);
    Task SendOrdreRéparationCrééEmailAsync(OrdreRéparation ordreRéparation, CancellationToken cancellationToken = default);
}