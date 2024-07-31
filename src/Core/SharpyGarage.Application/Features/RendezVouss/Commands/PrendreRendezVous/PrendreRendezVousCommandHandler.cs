using MediatR;
using Sharpy.SharpyGarage.Application.Abstractions;
using Sharpy.SharpyGarage.Application.Features.RendezVouss.Commands.ConfirmerRendezVous;
using Sharpy.SharpyGarage.Domain.Entities;
using Sharpy.SharpyGarage.Domain.Repositories;

namespace Sharpy.SharpyGarage.Application.Features.RendezVouss.Commands.PrendreRendezVous;

internal sealed class PrendreRendezVousCommandHandler : IRequestHandler<PrendreRendezVousCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IGarageRepository _garageRepository;
    private readonly IVéhiculeRepository _véhiculeRepository;
    private readonly IPrestationRepository _prestationRepository;
    private readonly IRendezVousRepository _rendezVousRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;


    public PrendreRendezVousCommandHandler(IClientRepository clientRepository,
                                           IGarageRepository garageRepository,
                                           IVéhiculeRepository véhiculeRepository,
                                           IPrestationRepository prestationRepository,
                                           IRendezVousRepository rendezVousRepository,
                                           IUnitOfWork unitOfWork,
                                           IEmailService emailService)
    {
        _garageRepository = garageRepository;
        _clientRepository = clientRepository;
        _véhiculeRepository = véhiculeRepository;
        _prestationRepository = prestationRepository;
        _rendezVousRepository = rendezVousRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(PrendreRendezVousCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        var garage = await _garageRepository.GetByIdAsync(request.GarageId, cancellationToken);
        var véhicule = await _véhiculeRepository.GetByIdAsync(request.VéhiculeId, cancellationToken);
        var prestation = await _prestationRepository.GetByIdAsync(request.PrestationId, cancellationToken);

        if (client is null || garage is null || véhicule is null || prestation is null)
        {
            return;
        }

        // Crée un RDV
        var rendezVous = new RendezVous
        {
            Id = Guid.NewGuid(),
            Created = DateTimeOffset.Now,
            LastModified = DateTimeOffset.Now,
            Date = request.Date,
            Client = client,
            Véhicule = véhicule,
            Prestation = prestation,
            Garage = garage,
            Statut = RendezVousStatut.EnAttente
        };

        // Validation
        if (request.Date < DateTimeOffset.Now)
        {
            throw new Exception($"Un rendez-vous ne peut pas être créé dans le passé.");
        }

        DateTime début = rendezVous.Date;
        DateTime fin = rendezVous.Date.Add(prestation.Durée);

        bool estPendantOuverture = garage.Horaires
                                    .Where(h => h.JourDeSemaine.Equals(rendezVous.Date.DayOfWeek))
                                    .Any(h => TimeOnly.FromDateTime(début) >= h.Ouverture && TimeOnly.FromDateTime(fin) <= h.Fermeture);

        if (!estPendantOuverture)
            throw new Exception($"Le garage n'est pas ouvert sur le créneau horaire demandé du rendez-vous.");

        int nbRdvPendantLeCréneauDemandé = garage.RendezVousList.Where(r => r.Date < fin && r.Date.Add(r.Prestation.Durée) > début).Count();

        bool estDisponible = nbRdvPendantLeCréneauDemandé < garage.NombreMécanicien;

        if (!estDisponible)
            throw new Exception($"Le créneau horaire demandé pour le rendez-vous n'est pas disponible.");

        // Ajout du rendez-vous
        garage.RendezVousList.Add(rendezVous);
        _rendezVousRepository.Add(rendezVous);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Envoi d'email de prise en compte de la demande de RDV
        if (rendezVous.Statut == RendezVousStatut.EnAttente)
        {
            await _emailService.SendDemandeRendezVousEmailAsync(rendezVous, cancellationToken);
        }
    }
}
