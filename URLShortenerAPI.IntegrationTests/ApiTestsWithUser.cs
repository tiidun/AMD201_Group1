using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Newtonsoft.Json;

using NUnit.Framework;

using URLShortenerAPI.Models.URLAddress;

using URLShortenerData.Data.Entities;

namespace UrlShotenerAPI.IntegrationTests
{
    [TestFixture]
    public class ApiTestsWithUser : ApiTestsBase
    {
        private IdentityUser maria;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await base.AuthenticateAsync();

            this.maria = this.testDb.UserMaria;
        }

        [Test]
        public async Task Test_Addresses_GetAddressSearch_ReturnsOk()
        {

        }

        [Test]
        public async Task Test_CreateAddress_ShouldCreateCorrectly()
        {

        }

        [Test]
        public async Task Test_DeleteAddress_ShouldDeleteCorrectly()
        {

        }
    }
}
