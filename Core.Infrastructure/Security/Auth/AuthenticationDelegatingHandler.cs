using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;

namespace Core.Infrastructure.Security.Auth;

public sealed class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        const string AuthorizationHeaderName = "Authorization";

        StringValues authorizationHeader;
        // Obtém o token do request atual
        if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(
            AuthorizationHeaderName, out authorizationHeader) is true)
        {
            if (AuthenticationHeaderValue.TryParse(authorizationHeader.ToString(), out var authValue))
            {
                // Adiciona/Substitui o token ao request que será enviado
                request.Headers.Authorization = authValue;
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
