using MediatR;

namespace Sharpy.SharpyGarage.Application.Features.RendezVouss.Commands.ConfirmerRendezVous;

public sealed record PrendreRendezVousCommand(DateTime Date,
                                                 Guid GarageId,
                                                 Guid ClientId,
                                                 Guid VéhiculeId,
                                                 Guid PrestationId,
                                                 Guid? RendezVousId) : IRequest;
