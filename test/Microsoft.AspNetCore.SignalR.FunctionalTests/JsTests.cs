using Microsoft.AspNetCore.SignalR.Testing.Common;
using Microsoft.AspNetCore.SignalR.Tests;
using Xunit;

namespace Microsoft.AspNetCore.SignalR.FunctionalTests
{
    public class JsTests
    {
        [Collection(FunctionalTestsCollection.Name)]
        public class HubTests
        {
            ServerFixture _serverFixture;

            public HubTests(FunctionalTestsServerFixture serverFixture)
            {
                _serverFixture = serverFixture;
            }

            [Fact]
            public void Run_javascript_tests()
            {
                Assert.Equal(0, Utils.RunPhantomJS(
                    // TODO: doesn't work if URL - investigate/find a better location
                    @"C:\source\SignalR-Server\test\Microsoft.AspNetCore.SignalR.Test.Server\wwwroot\js\run-jasmine.js",
                    _serverFixture.BaseUrl + "functionalTests.html"));
            }
        }
    }
}
