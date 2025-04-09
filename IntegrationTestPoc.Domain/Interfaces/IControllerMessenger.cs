namespace IntegrationTestPoc.Domain.Interfaces;
using IntegrationTestPoc.Domain.Utils;
public interface IControllerMessenger
{
    bool ErrorTriggered { get; }
    int StatusCode { get; }
    object? ResponseObject { get; }
    public ControllerMessenger ReturnSuccess(int statusCode, string message);
    ControllerMessenger ReturnSuccess(int statusCode, object returnObject);
    ControllerMessenger ReturnBadRequest400(string? message);
    ControllerMessenger ReturnNotFound404(string element);
    ControllerMessenger ReturnNotFound404();
    ControllerMessenger ReturnInternalError500(string ex);
    ControllerMessenger ReturnServiceUnavailable503();
}