namespace Sharpy.SharpyGarage.Domain.Entities;

public class Prestation
{
    public Guid Id { get; set; }
    public string Libellé { get; set; }
    public double Tarif { get; set; }
    public TimeSpan Durée { get; set; }
}
