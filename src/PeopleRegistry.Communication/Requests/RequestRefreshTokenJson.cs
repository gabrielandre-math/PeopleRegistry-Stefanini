namespace PeopleRegistry.Communication.Requests
{
    public class RequestRefreshTokenJson
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
