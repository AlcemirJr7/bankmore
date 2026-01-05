using System.Net.Http.Json;

namespace Core.Infrastructure.Extensions;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> GetWithHeadersAsync(
        this HttpClient client,
        string requestUri,
        Dictionary<string, string> headers,
        CancellationToken ct = default)
    {
        return SendWithHeadersAsync(client, HttpMethod.Get, requestUri, null, headers, ct);
    }

    public static Task<HttpResponseMessage> PostAsJsonWithHeadersAsync<T>(
        this HttpClient client,
        string requestUri,
        T value,
        Dictionary<string, string> headers,
        CancellationToken ct = default)
    {
        return SendWithHeadersAsync(client, HttpMethod.Post, requestUri, JsonContent.Create(value), headers, ct);
    }

    public static Task<HttpResponseMessage> PutAsJsonWithHeadersAsync<T>(
        this HttpClient client,
        string requestUri,
        T value,
        Dictionary<string, string> headers,
        CancellationToken ct = default)
    {
        return SendWithHeadersAsync(client, HttpMethod.Put, requestUri, JsonContent.Create(value), headers, ct);
    }

    public static Task<HttpResponseMessage> DeleteWithHeadersAsync(
        this HttpClient client,
        string requestUri,
        Dictionary<string, string> headers,
        CancellationToken ct = default)
    {
        return SendWithHeadersAsync(client, HttpMethod.Delete, requestUri, null, headers, ct);
    }

    private static async Task<HttpResponseMessage> SendWithHeadersAsync(
        HttpClient client,
        HttpMethod method,
        string requestUri,
        HttpContent? content,
        Dictionary<string, string> headers,
        CancellationToken ct)
    {
        var request = new HttpRequestMessage(method, requestUri)
        {
            Content = content
        };

        foreach (var header in headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return await client.SendAsync(request, ct);
    }
}
