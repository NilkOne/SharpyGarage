namespace Sharpy.SharpyGarage.Domain.Entities;

public class Véhicule
{
    public Guid Id { get; set; }
    public Client Propriétaire { get; set; }
    public string Immatriculation { get; set; }
    public DateTime MiseEnCirculation { get; set; }
    public string Marque { get; set; }
    public string Modèle { get; set; }
}