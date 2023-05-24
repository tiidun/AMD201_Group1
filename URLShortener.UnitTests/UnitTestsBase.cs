using NUnit.Framework;

using URLShortener.Tests.Common;

using URLShortenerData.Data;

namespace URLShortener.WebApp.UnitTests
{
    public class UnitTestsBase
    {
        protected TestDb testDb;
        protected URLShortenerDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetupBase()
        {
            this.testDb = new TestDb();
            this.dbContext = this.testDb.CreateDbContext();
        }
    }
}
