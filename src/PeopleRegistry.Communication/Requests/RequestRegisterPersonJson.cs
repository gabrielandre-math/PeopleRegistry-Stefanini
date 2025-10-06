using PeopleRegistry.Domain.Enums;

namespace PeopleRegistry.Communication.Requests;
public class RequestRegisterPersonJson
{
    public string Name { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public string? PlaceOfBirth { get; set; }  // Naturalidade
    public string? Nationality { get; set; }   // Nacionalidade
}
