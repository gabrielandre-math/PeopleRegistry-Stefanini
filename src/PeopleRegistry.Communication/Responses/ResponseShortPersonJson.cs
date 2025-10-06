namespace PeopleRegistry.Communication.Responses;
public class ResponseShortPersonJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}
