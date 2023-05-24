using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using URLShortener.Tests.Common;

namespace URLShortener.Tests.Common
{
    public class TestUrlShortenerApp<TEntryPoint> : TestUrlShortenerBase<TEntryPoint>
        where TEntryPoint : class
    {
        private IHost builtInHost;

        public TestUrlShortenerApp(TestDb testDb)
            : base(testDb)
        {
        }

        public string HostUrl => "http://localhost:8080";

        protected override IHost CreateHost(IHostBuilder builder)
        {
            this.builtInHost = builder.Build();

            builder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder = ConfigureServices(webHostBuilder);
                webHostBuilder.UseUrls(HostUrl);
                webHostBuilder.UseKestrel();
            });

            IHost customHost = builder.Build();
            customHost.Start();

            return this.builtInHost;
        }

        public void Dispose() => this.builtInHost.Dispose();
    }
}
