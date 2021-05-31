using System;
using System.Threading.Tasks;
using Xunit;

namespace BTBConnectorTests
{
    public class ClientConnectionServiceTests
    {
        [Fact]
        public async Task Test1()
        {
            var currentDate = DateTime.Now;
            var service = TestHelpers.CreateClintConnectorService();
            var result = await service.DownloadDataDailyAsync(currentDate);

            Assert.NotNull(result);
        }
    }
}
