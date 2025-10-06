namespace PeopleRegistry.Communication.Requests;

public class AddressJson
{
    public string Street { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}

public class RequestRegisterPersonV2Json : RequestRegisterPersonJson
{
    public AddressJson Address { get; set; } = null!;
}
