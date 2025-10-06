namespace PeopleRegistry.Communication.Responses;

public class ResponseErrorJson
{
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public ResponseErrorJson(string message)
    {
        Message = message;
        Errors = [];
    }
    public ResponseErrorJson(string message, List<string> errors)
    {
        Message = message;
        Errors = errors;
    }
}
