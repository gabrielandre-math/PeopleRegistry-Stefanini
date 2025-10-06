using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeopleRegistry.Communication.Responses;
using PeopleRegistry.Exception;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception");

        if (context.Exception is PeopleRegistryException ex)
        {
            var status = (int)ex.GetStatusCode();
            ResponseErrorJson body;

            if (ex is ErrorOnValidationException ev)
                body = new ResponseErrorJson(ev.Message, ev.GetErrors());
            else
                body = new ResponseErrorJson(ex.Message);

            context.HttpContext.Response.StatusCode = status;
            context.Result = new JsonResult(body);
            return;
        }

        // Em DEV retornamos detalhes para depurar
        var message = _env.IsDevelopment()
            ? context.Exception.ToString()
            : "An unexpected error occurred.";

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new JsonResult(new ResponseErrorJson(message));
    }
}
