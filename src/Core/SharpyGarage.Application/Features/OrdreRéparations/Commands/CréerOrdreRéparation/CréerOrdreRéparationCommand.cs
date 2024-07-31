using MediatR;

namespace Sharpy.SharpyGarage.Application.Features.OrdreRéparations.Commands.CréerOrdreRéparation;

public sealed record CréerOrdreRéparationCommand(DateTime Date,
                                                 Guid GarageId,
                                                 Guid ClientId,
                                                 Guid VéhiculeId,
                                                 Guid PrestationId,
                                                 Guid? RendezVousId) : IRequest;
