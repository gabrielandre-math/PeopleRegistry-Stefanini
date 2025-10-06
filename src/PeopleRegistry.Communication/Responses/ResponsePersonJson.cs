using PeopleRegistry.Domain.Enums;

namespace PeopleRegistry.Communication.Responses;
public class ResponsePersonJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public string? PlaceOfBirth { get; set; }
    public string? Nationality { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
