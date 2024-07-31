namespace Sharpy.SharpyGarage.Domain.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string Nom { get; set; }
    public string Prénom { get; set; }
    public string Email { get; set; }
}