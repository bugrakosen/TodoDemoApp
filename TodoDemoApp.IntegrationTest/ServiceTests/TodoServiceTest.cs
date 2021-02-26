using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TodoDemoApp.API.DTOs;
using TodoDemoApp.API.Seed;
using Xunit;

namespace TodoDemoApp.IntegrationTest.ServiceTests
{
    public class TodoServiceTest
    {
        private readonly HttpClient _httpclient;

        public TodoServiceTest()
        {
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<FakeStartup>()
                .UseEnvironment("Development"));
            _httpclient = testServer.CreateClient();
        }

        [Theory]
        [InlineData("todos")]
        public async Task TestTodo(string url)
        {
            await SeedData.ResetDatas().ConfigureAwait(false);

            var responseGet = await _httpclient.GetAsync(url).ConfigureAwait(false);

            var result = await responseGet.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<List<TodoDTO>>(result);

            var expectedCount = 10;

            Assert.Equal(expectedCount, response.Count);
        }
    }
}
