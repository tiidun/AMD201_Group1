using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortenerAPI.Models.URLAddress;
using URLShortenerData.Data;
using URLShortenerData.Data.Entities;
using URLShortenerAPI.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace URLShortenerAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/urladdresses")]
    public class URLAddressController : Controller
    {
        private readonly URLShortenerDbContext data;
        public URLAddressController(URLShortenerDbContext data)
            => this.data = data;

        /// <summary>
        /// Gets url addresses count. If you are a logged in user, you will also see your URL count.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/urladdresses/count
        ///     {
        ///
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with url addresses count</response>
        [HttpGet("count")]
        [AllowAnonymous]
        public IActionResult GetURLAddressesCount()
        {
            URLAddressesCount urlAddressesModel = new URLAddressesCount();

            urlAddressesModel.AllURLAddressesCount = this.data.URLAddresses.Count();

            if (this.User.Identity.IsAuthenticated)
            {
                urlAddressesModel.UserURLAddressesCount = this.data.URLAddresses
                    .Where(x => x.UserId == GetCurrentUserId()).Count();
            };

            return Ok(urlAddressesModel);
        }

        /// <summary>
        /// Gets a list with all url addresses.
        /// </summary>
        /// <remarks>
        ///
        /// Sample request:
        ///
        ///     GET /api/urladdresses
        ///     {
        ///
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with a list of url addresses</response>
        /// <response code="204">Returns "No URL Addresses" when there are no url addresses</response>
        [HttpGet()]
        [AllowAnonymous]
        public IActionResult GetURLAddresses()
        {
            var urlAddresses = this.data.URLAddresses.Select(x => new URLAddressesListingModel()
            {
                Id = x.Id,
                OriginalUrl = x.OriginalUrl,
                ShortUrl = x.ShortUrl,
                DateCreated = x.DateCreated,
                Visits = x.Visits
            })
            .ToList();

            if (urlAddresses.Count == 0)
            {
                return NoContent();
            }

            return Ok(urlAddresses);
        }

        /// <summary>
        /// Search for a url address by keyword.
        /// </summary>
        /// <remarks>
        ///
        /// Sample request:
        ///
        ///     GET /api/urladdresses/search/{keyword}
        ///     {
        ///
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with the matching urladdress</response>
        /// <response code="204">Returns "NoContent" when no such url address exists</response>
        [HttpGet("search/{keyword}")]
        [AllowAnonymous]
        public IActionResult GetURLAddressByOriginalUrl(string? keyword)
        {

            var urlAddress = this.data.URLAddresses.Select(x => new URLAddressesListingModel()
            {
                Id = x.Id,
                OriginalUrl = x.OriginalUrl,
                ShortUrl = x.ShortUrl,
                DateCreated = x.DateCreated,
                Visits = x.Visits
            })
            .ToList();

            URLAddressesListingModel searchedUrl = new URLAddressesListingModel();

            keyword = keyword == null ? string.Empty : keyword.Trim().ToLower();

            if (!String.IsNullOrEmpty(keyword))
            {
                string keywordToValidUrl = keyword.Replace("%2f%2f", "//");
                searchedUrl = urlAddress.FirstOrDefault(x => x.OriginalUrl.ToLower().Contains(keywordToValidUrl.ToLower()));
            }

            if (searchedUrl == null)
            {
                return NoContent();
            }

            return Ok(searchedUrl);
        }

        /// <summary>
        /// Creates new short url.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        ///
        /// Sample request:
        ///
        ///     POST /api/urladdresses/create
        ///     {
        ///            "URL": "https://www.google.com",
        ///            "Short Code": "goo"
        ///     }
        /// </remarks>
        /// <response code="201">Returns "Created" with the created event</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>
        [HttpPost("create")]
        public IActionResult CreateURLAddress(URLAddressBindingModel urlModel)
        {
            URLAddress urlExist = this.data
                .URLAddresses
                .FirstOrDefault(x => x.OriginalUrl == urlModel.URL);

            if (urlExist != null)
            {
                return BadRequest(new ResponseMsg()
                {
                    Message = "This URL already exists!"
                });
            }

            string currentLoggedInUserId = GetCurrentUserId();

            URLAddress url = new URLAddress
            {
                OriginalUrl = urlModel.URL,
                ShortUrl = "http://shorturl.nakov.repl.co/go/" + urlModel.ShortCode,
                DateCreated = DateTime.UtcNow,
                UserId = currentLoggedInUserId,
                Visits = 0
            };

            this.data.URLAddresses.Add(url);
            this.data.SaveChanges();


            var resultModel = CreateURLAddressExtendedModel(url);

            return new ObjectResult(resultModel) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Edits a url address.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the url address!
        ///
        /// Sample request:
        ///
        ///     PUT /api/urladdresses/{originalURL}
        ///     {
        ///            "URL": "https://www.google.com",
        ///            "Short Code": "goo"
        ///     }
        /// </remarks>
        /// <response code="204">Returns "No Content"</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the url</response>
        /// <response code="404">Returns "Not Found" when the url address with the given original url doesn't exist</response>
        [HttpPut("{originalURL}")]
        public IActionResult PutContact(URLAddressBindingModel urlModel)
        {
            URLAddress urlExist = this.data
                .URLAddresses
                .FirstOrDefault(x => x.OriginalUrl == urlModel.URL);

            if (urlExist == null)
            {
                return NotFound(
                    new ResponseMsg { Message = $"URL {urlModel.URL} was not found." });
            }

            var user = this.data.Users.FirstOrDefault(x => x.Id == GetCurrentUserId());

            if (urlExist.UserId != user.Id)
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot edit url, when you are not its owner." });
            }

            // Get urls of given user, except the current one
            var userUrls = this.data.URLAddresses.Where(x => x.UserId == user.Id && x.OriginalUrl != urlModel.URL);

            if (userUrls.Any(c => c.OriginalUrl == urlModel.URL))
            {
                return BadRequest(new ResponseMsg()
                {
                    Message = "You already have a short URL of that original URL!"
                });
            }

            urlExist.ShortUrl = urlModel.ShortCode;
            urlExist.OriginalUrl = urlModel.URL;

            this.data.SaveChanges();

            var editedURLModel = CreateURLAddressExtendedModel(urlExist);

            return Ok(editedURLModel);
        }

        /// <summary>
        /// Deletes URL.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the deleted URL!
        ///
        /// Sample request:
        ///
        ///     DELETE /api/urladdresses/{id}
        ///     {
        ///
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with the deleted url</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the url</response>
        /// <response code="404">Returns "Not Found" when url with the given id doesn't exist</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            URLAddress? url = this.data.URLAddresses.FirstOrDefault(x => x.Id == id);
            if (url == null)
            {
                return this.NotFound(
                    new ResponseMsg { Message = $"URL with id {id} was not found." });
            }

            IdentityUser user = this.data.Users.FirstOrDefault(x => x.Id == this.GetCurrentUserId())!;
            if (url.UserId != user.Id)
            {
                return this.Unauthorized(
                    new ResponseMsg { Message = "Cannot delete url, when you are not its owner." });
            }

            this.data.URLAddresses.Remove(url);
            this.data.SaveChanges();

            URLAddressesListingModel deletedURLModel = CreateURLAddressExtendedModel(url);
            return this.Ok(deletedURLModel);
        }

        private static URLAddressesListingModel CreateURLAddressExtendedModel(URLAddress url)
          => new URLAddressesListingModel()
          {
              Id = url.Id,
              OriginalUrl = url.OriginalUrl,
              ShortUrl = url.ShortUrl,
              DateCreated = url.DateCreated,
              Visits = url.Visits
          };

        private string GetCurrentUserId()
        {
            string currentUsername = this.User.Identity.Name;
            var currentUserId = this.data
                .Users
                .FirstOrDefault(x => x.UserName == currentUsername)
                .Id;
            return currentUserId;
        }

    }
}
