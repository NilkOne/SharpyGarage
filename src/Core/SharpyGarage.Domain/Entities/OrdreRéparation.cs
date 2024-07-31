namespace Sharpy.SharpyGarage.Domain.Entities;

public class OrdreRéparation
{
    public Guid Id { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public DateTime Date { get; set; }
    public Guid? RendezVousId { get; set; }
    public Client? Client { get; set; }
    public Véhicule Véhicule { get; set; }
    public Prestation Prestation { get; set; }
    public Garage Garage { get; set; }
    public OrdreRéparationStatut Statut { get; set; }
    public List<Mécanicien> Mécaniciens { get; set; }
}
