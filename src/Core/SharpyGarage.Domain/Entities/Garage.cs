using Sharpy.SharpyGarage.Domain.ValueObjects;

namespace Sharpy.SharpyGarage.Domain.Entities;

public class Garage
{
    public Guid Id { get; set; }
    public string Nom { get; set; }
    public Adresse Adresse { get; set; }
    public string Email { get; set; }
    public List<RendezVous> RendezVousList { get; set; }
    public List<HoraireOuverture> Horaires { get; set; }
    public int NombreMécanicien { get; set; }
}
