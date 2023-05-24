using System;
using System.Collections.ObjectModel;

using NUnit.Framework;

using OpenQA.Selenium;

namespace URLShortener.SeleniumTests
{
    public class SeleniumTestsAddresses : SeleniumTestsBase
    {
        private Random random;
        private int randomNumber;

        [OneTimeSetUp]
        public void OneTimeSetUpUser()
        {
            this.RegisterUserForTesting();
        }

        [SetUp]
        public void SetUpRandom()
        {
            this.random = new Random();
            this.randomNumber = random.Next(10000, 90000);
        }

        [Test]
        public void Test_CreateAddress_ValidData()
        {

        }

        [Test]
        public void Test_DeleteAddress_DeletesCorrectly()
        {

        }

        [Test]
        public void Test_EditAddress_EditsCorrectly()
        {

        }

        private void RegisterUserForTesting()
        {

        }

        private void CreateUrl(out string url)
        {

        }
    }
}
