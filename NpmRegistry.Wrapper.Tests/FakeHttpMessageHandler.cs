namespace NpmRegistry.Wrapper.Tests;

using System.Net;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly HttpContent _httpContent;

    public Uri? LastRequestUri { get; private set; }

    public FakeHttpMessageHandler(string responseBody)
    {
        _statusCode = HttpStatusCode.OK;
        _httpContent = new StringContent(responseBody);
    }

    public FakeHttpMessageHandler(HttpStatusCode statusCode, HttpContent httpContent)
    {
        _statusCode = statusCode;
        _httpContent = httpContent;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = Send(request, cancellationToken);
        return Task.FromResult(response);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequestUri = request.RequestUri;

        return new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = _httpContent
        };
    }
}