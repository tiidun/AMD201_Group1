using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Identity;

using NUnit.Framework;

using URLShortener.Tests.Common;

using URLShortenerAPI.Models.Response;
using URLShortenerAPI.Models.User;

using URLShortenerData.Data;

namespace UrlShotenerAPI.IntegrationTests
{
    public class ApiTestsBase
    {
        protected TestDb testDb;
        protected URLShortenerDbContext dbContext;
        protected TestUrlShortenerApi<Program> testUrlShortenerApi;
        protected HttpClient httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.testDb = new TestDb();
            this.dbContext = this.testDb.CreateDbContext();
            this.testUrlShortenerApi = new TestUrlShortenerApi<Program>(this.testDb);
            this.httpClient = this.testUrlShortenerApi.CreateFactory().CreateClient();
        }

        public async Task AuthenticateAsync()
        {
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await this.GetJWTAsync());
        }

        private async Task<string> GetJWTAsync()
        {
            IdentityUser userMaria = this.testDb.UserMaria;
            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("/api/users/login",
                new LoginModel
                {
                    Email = userMaria.Email,
                    Password = userMaria.UserName
                });

            ResponseWithToken loginResponse = await response.Content.ReadFromJsonAsync<ResponseWithToken>();

            return loginResponse.Token;
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the local Web API server
            this.testUrlShortenerApi.Dispose();
        }
    }
}
