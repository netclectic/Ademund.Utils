using System.Collections.Generic;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Ademund.Utils.Net.Tests
{
    public class IPUtilsTests
    {
        [Theory]
        [InlineData(8)]
        public void GetExternalIPAddress_Responds_With_A_Valid_IPAddress(int testNum)
        {
            using (var server = WireMockServer.Start())
            {
                var urls = new List<string>(testNum);
                for (int i = 1; i < testNum + 1; i++)
                {
                    urls.Add($"{server.Urls[0]}/test/{i}");
                    server.Given(Request.Create().WithPath($"/test/{i}").UsingGet())
                        .RespondWith(Response.Create().WithStatusCode(200).WithBody($"192.168.0.{i}"));
                }

                var response = IPUtils.GetExternalIPAddress(checkIPUrls: urls.ToArray());

                Assert.NotNull(response);
                Assert.StartsWith("192.168.0.", response);
            }
        }

        [Theory]
        [InlineData(7, 8)]
        public void GetExternalIPAddress_Does_Not_Throw_If_Requests_Fail(int failingTestsNum, int passingTest)
        {
            using (var server = WireMockServer.Start())
            {
                var urls = new List<string>(failingTestsNum);
                for (int i = 1; i < failingTestsNum + 1; i++)
                {
                    urls.Add($"{server.Urls[0]}/test/{i}");
                    server.Given(Request.Create().WithPath($"/test/{i}").UsingGet())
                        .RespondWith(Response.Create().WithStatusCode(400).WithBody($"Internal Server Error"));
                }

                urls.Add($"{server.Urls[0]}/test/{passingTest}");
                server.Given(Request.Create().WithPath($"/test/{passingTest}").UsingGet())
                    .RespondWith(Response.Create().WithStatusCode(200).WithBody($"192.168.0.{passingTest}"));

                var response = IPUtils.GetExternalIPAddress(checkIPUrls: urls.ToArray());

                Assert.NotNull(response);
                Assert.Equal($"192.168.0.{passingTest}", response);
            }
        }

        [Theory]
        [InlineData(8)]
        public void GetExternalIPAddress_Returns_Null_If_All_Requests_Fail(int failingTestsNum)
        {
            using (var server = WireMockServer.Start())
            {
                var urls = new List<string>(failingTestsNum);
                for (int i = 1; i < failingTestsNum + 1; i++)
                {
                    urls.Add($"{server.Urls[0]}/test/{i}");
                    server.Given(Request.Create().WithPath($"/test/{i}").UsingGet())
                        .RespondWith(Response.Create().WithStatusCode(400).WithBody($"Internal Server Error"));
                }

                var response = IPUtils.GetExternalIPAddress(checkIPUrls: urls.ToArray(), failDelay: 1000);

                Assert.Null(response);
            }
        }
    }
}
