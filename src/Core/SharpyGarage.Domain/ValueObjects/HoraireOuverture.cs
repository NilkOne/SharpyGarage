namespace Sharpy.SharpyGarage.Domain.ValueObjects;

public class HoraireOuverture
{
    public DayOfWeek JourDeSemaine { get; set; }
    public TimeOnly Ouverture { get; set; }
    public TimeOnly Fermeture { get; set; }
}
