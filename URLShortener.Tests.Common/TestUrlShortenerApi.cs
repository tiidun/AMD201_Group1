using Microsoft.AspNetCore.Mvc.Testing;

namespace URLShortener.Tests.Common
{
    public class TestUrlShortenerApi<TEntryPoint> : TestUrlShortenerBase<TEntryPoint>
        where TEntryPoint : class
    {
        private WebApplicationFactory<TEntryPoint> factory;

        public TestUrlShortenerApi(TestDb testDb)
            : base(testDb)
        {
        }

        public WebApplicationFactory<TEntryPoint> CreateFactory()
        {
            this.factory = new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(webHostBuilder =>
                    ConfigureServices(webHostBuilder));

            return this.factory;
        }

        public void Dispose() => this.factory.Dispose();
    }
}
