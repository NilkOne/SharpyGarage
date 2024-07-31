using MediatR;
using Sharpy.SharpyGarage.Application.Abstractions;
using Sharpy.SharpyGarage.Domain.Entities;
using Sharpy.SharpyGarage.Domain.Repositories;

namespace Sharpy.SharpyGarage.Application.Features.OrdreRéparations.Commands.CréerOrdreRéparation;

internal sealed class CréerOrdreRéparationCommandHandler : IRequestHandler<CréerOrdreRéparationCommand>
{
    private readonly IGarageRepository _garageRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IVéhiculeRepository _véhiculeRepository;
    private readonly IPrestationRepository _prestationRepository;
    private readonly IRendezVousRepository _rendezVousRepository;
    private readonly IOrdreRéparationRepository _ordreRéparationRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public CréerOrdreRéparationCommandHandler(IGarageRepository garageRepository,
                                              IClientRepository clientRepository,
                                              IVéhiculeRepository véhiculeRepository,
                                              IPrestationRepository prestationRepository,
                                              IRendezVousRepository rendezVousRepository,
                                              IOrdreRéparationRepository ordreRéparationRepository,
                                              IUnitOfWork unitOfWork,
                                              IEmailService emailService)
    {
        _garageRepository = garageRepository;
        _clientRepository = clientRepository;
        _véhiculeRepository = véhiculeRepository;
        _prestationRepository = prestationRepository;
        _rendezVousRepository = rendezVousRepository;
        _ordreRéparationRepository = ordreRéparationRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(CréerOrdreRéparationCommand request, CancellationToken cancellationToken)
    {
        var garage = await _garageRepository.GetByIdAsync(request.GarageId, cancellationToken);
        var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        var véhicule = await _véhiculeRepository.GetByIdAsync(request.VéhiculeId, cancellationToken);
        var prestation = await _prestationRepository.GetByIdAsync(request.PrestationId, cancellationToken);

        if (garage is null || véhicule is null || prestation is null)
        {
            return;
        }

        // Crée un OR
        var ordreRéparation = new OrdreRéparation
        {
            Id = Guid.NewGuid(),
            Created = DateTimeOffset.Now,
            LastModified = DateTimeOffset.Now,
            Date = request.Date,
            RendezVousId = request.RendezVousId,
            Client = client,
            Véhicule = véhicule,
            Prestation = prestation,
            Garage = garage,
            Statut = OrdreRéparationStatut.ATraiter
        };

        // Validation
        if (request.Date < DateTimeOffset.Now)
        {
            throw new Exception($"Un ordre de réparation ne peut pas être créé dans le passé.");
        }

        // Ajout de l'ordre de réparation
        _ordreRéparationRepository.Add(ordreRéparation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Envoi d'email de confirmation de création de l'OR
        if (ordreRéparation.Statut == OrdreRéparationStatut.ATraiter)
        {
            await _emailService.SendOrdreRéparationCrééEmailAsync(ordreRéparation, cancellationToken);
        }
    }
}
