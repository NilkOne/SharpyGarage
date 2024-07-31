namespace Sharpy.SharpyGarage.Domain.Entities;

public class Mécanicien
{
    public Guid Id { get; set; }
    public Guid GarageId { get; set; }
    public string Nom { get; set; }
    public string Prénom { get; set; }
}
