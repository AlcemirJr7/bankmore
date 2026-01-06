using Core.Abstractions;
using Core.Response;
using Core.Security.Errors;

namespace Core.Exceptions;

public class UnauthorizedUserApiException : Exception, IExceptionApi<ErrorDetails>
{
    public ErrorDetails ToError() => AuthErrors.Login.UnauthorizedUser;
}