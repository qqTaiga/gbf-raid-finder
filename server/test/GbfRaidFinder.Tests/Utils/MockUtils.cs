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
    public static HttpClient MockHttpClient(string returnContent)
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(returnContent)
            });
        HttpClient httpClient = new(mockMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://localhost");

        return httpClient;
    }
}
