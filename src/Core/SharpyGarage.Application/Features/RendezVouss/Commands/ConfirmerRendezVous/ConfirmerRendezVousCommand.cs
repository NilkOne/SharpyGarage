using MediatR;

namespace Sharpy.SharpyGarage.Application.Features.RendezVouss.Commands.ConfirmerRendezVous;

public sealed record ConfirmerRendezVousCommand(Guid RendezVousId) : IRequest;
