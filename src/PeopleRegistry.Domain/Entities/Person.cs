using PeopleRegistry.Domain.Enums;

namespace PeopleRegistry.Domain.Entities;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public string? PlaceOfBirth { get; set; }  // Naturalidade
    public string? Nationality { get; set; }   // Nacionalidade
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
