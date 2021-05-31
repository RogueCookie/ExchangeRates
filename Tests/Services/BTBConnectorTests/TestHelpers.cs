using System;
using System.Net.Http;
using BTBConnector.Interfaces;
using BTBConnector.Services;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BTBConnectorTests
{
    public static class TestHelpers
    {
        public static IClientConnectorService CreateClintConnectorService()
        {
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://www.cnb.cz")
            };
            mockHttpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            return new ClientConnectorService(mockHttpFactory.Object, new NullLogger<ClientConnectorService>());
        }
    }
}