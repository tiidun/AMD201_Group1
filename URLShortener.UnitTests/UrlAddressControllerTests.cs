using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using URLShortener.Controllers;
using URLShortener.Models.URLAddresses;
using URLShortener.Tests.Common;
using URLShortener.WebApp.UnitTests;

using URLShortenerData.Data.Entities;

namespace UrlShortener.WebApp.UnitTests
{
    public class UrlAddressControllerTests : UnitTestsBase
    {
        private URLAddressController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Instantiate the controller class with the testing database
            this.controller = new URLAddressController(
                this.testDb.CreateDbContext());
            // Set UserMaria as current logged user
            TestingUtils.AssignCurrentUserForController(this.controller, this.testDb.UserMaria);
        }

        [Test]
        public void Test_All()
        {

        }

        [Test]
        public void Test_Create_PostInvalidData()
        {

        }

        [Test]
        public void Test_Delete_NonExistentId()
        {

        }

        public void Test_Edit_PostValidData()
        {

        }
    }
}
