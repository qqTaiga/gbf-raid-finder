using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace GbfRaidFinder.Tests.Utils;

public static class MockUtils
{
    /// <summary>
    /// Create a mocked <c>HttpClient</c>, returned content and status code are based on parameter.
    /// </summary>
    /// <param name="returnContent">Content that need to return by <c>HttpClient</c></param>
    /// <param name="statusCode">Content that need to return by <c>HttpClient</c></param>
    /// <returns>
    /// Mocked <c>HttpClient</c>
    /// </returns>
    public static HttpClient MockHttpClient(string returnContent, HttpStatusCode statusCode)
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(returnContent)
            });
        HttpClient httpClient = new(mockMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://localhost");

        return httpClient;
    }
}
