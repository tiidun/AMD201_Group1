using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using URLShortenerData.Data;
using URLShortenerData.Data.Entities;

namespace URLShortener.Tests.Common
{
    public class TestDb : IdentityDbContext
    {
        private readonly string uniqueDbName;

        public TestDb()
        {
            this.uniqueDbName = $"UrlShortener-TestDb-{DateTime.Now.Ticks}";
            this.SeedDatabase();
        }

        public URLShortenerDbContext CreateDbContext()
        {
            var optionsBuilder
                = new DbContextOptionsBuilder<URLShortenerDbContext>();

            optionsBuilder.UseInMemoryDatabase(this.uniqueDbName);
            return new URLShortenerDbContext(optionsBuilder.Options, false);
        }

        public URLAddress GoogleAddress { get; set; }

        public URLAddress StackOverflowAddress { get; set; }

        public URLAddress YoutubeAddress { get; set; }

        public IdentityUser GuestUser { get; set; }

        public IdentityUser UserMaria { get; set; }

        private void SeedDatabase()
        {
            URLShortenerDbContext dbContext = this.CreateDbContext();
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(dbContext);
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            UpperInvariantLookupNormalizer normalizer = new UpperInvariantLookupNormalizer();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(
                userStore, null, hasher, null, null, normalizer, null, null, null);

            // Create GuestUser
            this.GuestUser = new IdentityUser
            {
                UserName = "guest@mail.com",
                Email = "guest@mail.com",
                PhoneNumber = "+359882634611"
            };
            userManager.CreateAsync(this.GuestUser, this.GuestUser.UserName).Wait();

            // GoogleAddress has owner GuestUser
            this.GoogleAddress = new URLAddress
            {
                Id = 1,
                OriginalUrl = "https://www.google.com/",
                ShortUrl = "http://shorturl.nakov.repl.co/go/goog",
                DateCreated = DateTime.Now.AddDays(2),
                Visits = 100,
                UserId = this.GuestUser.Id,
                User = this.GuestUser,
            };
            dbContext.Add(this.GoogleAddress);

            // StackOverflowAddress has owner GuestUser
            this.StackOverflowAddress = new URLAddress
            {
                Id = 2,
                OriginalUrl = "https://stackoverflow.com/",
                ShortUrl = "http://shorturl.nakov.repl.co/go/stack",
                DateCreated = DateTime.Now.AddDays(5),
                Visits = 1000,
                UserId = this.GuestUser.Id,
                User = this.GuestUser,
            };
            dbContext.Add(this.StackOverflowAddress);

            // Create UserMaria
            this.UserMaria = new IdentityUser
            {
                UserName = "maria@gmail.com",
                Email = "maria@gmail.com",
                PhoneNumber = "+359882134611"
            };
            userManager.CreateAsync(this.UserMaria, this.UserMaria.UserName).Wait();

            // YoutubeAddress has owner UserMaria
            this.YoutubeAddress = new URLAddress
            {
                Id = 3,
                OriginalUrl = "https://youtube.com/",
                ShortUrl = "http://shorturl.nakov.repl.co/go/yt",
                DateCreated = DateTime.Now.AddDays(10),
                Visits = 1000,
                UserId = this.UserMaria.Id,
                User = this.UserMaria,
            };
            dbContext.Add(this.YoutubeAddress);

            dbContext.SaveChanges();
        }
    }
}
