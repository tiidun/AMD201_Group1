using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using URLShortenerData.Data;

namespace URLShortener.Tests.Common
{
    public class TestUrlShortenerBase<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        protected TestDb TestDb { get; set; }

        protected TestUrlShortenerBase(TestDb testDb)
            => this.TestDb = testDb;

        protected IWebHostBuilder ConfigureServices(IWebHostBuilder webHostBuilder)
            => webHostBuilder.ConfigureServices(services =>
            {
                ServiceDescriptor? oldDbContext = services.SingleOrDefault(
                        descr => descr.ServiceType == typeof(URLShortenerDbContext));
                services.Remove(oldDbContext);
                services.AddScoped<URLShortenerDbContext>(
                    provider => this.TestDb.CreateDbContext());
            });
    }
}
