using IntegrationTestPoc.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTestPoc.Domain.Utils;

public class ControllerMessenger : IControllerMessenger
{
    public ControllerMessenger()
    {
        SuccessMessage = new();
        Error = new();
    }
    public bool ErrorTriggered { get; private set; }
    public int StatusCode { get; private set; }
    public object? ResponseObject { get; private set; }
    private SuccessMessage SuccessMessage { get; set; }
    private ProblemDetails Error { get; set; }
    public ControllerMessenger ReturnSuccess(int statusCode, string message)
    {
        SuccessMessage.Status = statusCode;
        SuccessMessage.Message = message;

        StatusCode = (int)SuccessMessage.Status;
        ResponseObject = SuccessMessage;

        return this;
    }

    public ControllerMessenger ReturnSuccess(int statusCode, object returnObject)
    {
        StatusCode = statusCode;
        ResponseObject = returnObject;

        return this;
    }

    public ControllerMessenger ReturnBadRequest400(string? message)
    {
        Error.Detail = message ?? $"The server could not process the request";
        Error.Status = 400;
        Error.Title = "Bad Request";

        StatusCode = (int)Error.Status;
        ResponseObject = Error;
        ErrorTriggered = true;

        return this;
    }

    public ControllerMessenger ReturnNotFound404(string element)
    {
        Error.Detail = $"The element {element} was not found";
        Error.Status = 404;
        Error.Title = "Not Found";

        StatusCode = (int)Error.Status;
        ResponseObject = Error;
        ErrorTriggered = true;

        return this;
    }

    public ControllerMessenger ReturnNotFound404()
    {
        Error.Detail = $"Data was not found";
        Error.Status = 404;
        Error.Title = "Not Found";

        StatusCode = (int)Error.Status;
        ResponseObject = Error;
        ErrorTriggered = true;

        return this;
    }

    public ControllerMessenger ReturnInternalError500(string ex)
    {
        Error.Detail = $"{ex}";
        Error.Status = 500;
        Error.Title = "Internal Error";

        StatusCode = (int)Error.Status;
        ResponseObject = Error;
        ErrorTriggered = true;

        return this;
    }
    public ControllerMessenger ReturnServiceUnavailable503()
    {
        Error.Detail = $"Was not possible to complete your request";
        Error.Status = 503;
        Error.Title = "Service Unavailable";

        StatusCode = (int)Error.Status;
        ResponseObject = Error;
        ErrorTriggered = true;

        return this;
    }

}

public class SuccessMessage
{
    public int Status { get; set; }
    public string? Message { get; set; }
}