using MediatR;
using Sharpy.SharpyGarage.Application.Abstractions;
using Sharpy.SharpyGarage.Domain.Entities;
using Sharpy.SharpyGarage.Domain.Repositories;

namespace Sharpy.SharpyGarage.Application.Features.RendezVouss.Commands.ConfirmerRendezVous;

internal sealed class ConfirmerRendezVousCommandHandler : IRequestHandler<ConfirmerRendezVousCommand>
{
    private readonly IRendezVousRepository _rendezVousRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ConfirmerRendezVousCommandHandler(IRendezVousRepository rendezVousRepository,
                                             IUnitOfWork unitOfWork,
                                             IEmailService emailService)
    {
        _rendezVousRepository = rendezVousRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(ConfirmerRendezVousCommand request, CancellationToken cancellationToken)
    {
        var rendezVous = await _rendezVousRepository.GetByIdAsync(request.RendezVousId, cancellationToken);

        if (rendezVous is null || rendezVous.Statut != RendezVousStatut.EnAttente)
        {
            return;
        }

        var client = rendezVous.Client;

        // Vérification si expiré
        var expiré = rendezVous.Expiration is not null && rendezVous.Expiration < DateTimeOffset.UtcNow;

        if (expiré)
        {
            rendezVous.Statut = RendezVousStatut.Expiré;
            rendezVous.LastModified = DateTimeOffset.Now;
        }
        else
        {
            rendezVous.Statut = RendezVousStatut.Confirmé;
            rendezVous.LastModified = DateTimeOffset.Now;
        }

        // [QuestionRéponse] Créer un OR - Comment ?
        // ...

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Envoi d'email de confirmation du RDV
        if (rendezVous.Statut == RendezVousStatut.Confirmé)
        {
            await _emailService.SendAcceptationRendezVousEmailAsync(rendezVous, cancellationToken);
        }
    }
}
