using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using URLShortenerAPI.Models.Response;
using URLShortenerAPI.Models.User;
using URLShortenerData.Data;


namespace URLShortenerAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly URLShortenerDbContext data;
        private UserManager<IdentityUser> userManager;
        private readonly IConfiguration _configuration;
        public UsersController(URLShortenerDbContext data, IConfiguration configuration)
        {
            this.data = data;
            this._configuration = configuration;

            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(data);
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            UpperInvariantLookupNormalizer normalizer = new UpperInvariantLookupNormalizer();
            LoggerFactory factory = new LoggerFactory();
            Logger<UserManager<IdentityUser>> logger = new Logger<UserManager<IdentityUser>>(factory);

            this.userManager = new UserManager<IdentityUser>(
                    userStore, null, hasher, null, null, normalizer, null, null, logger);
        }

        /// <summary>
        /// Logs a user in. (Default user credentials: guest@mail.com / guest123)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/login
        ///     {
        ///        "email": "guest@mail.com",
        ///        "password": "guest123"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">Returns "OK" with JWT token with expiration date.</response>
        /// <response code="401">Returns "Unauthorized" when username or password doesn't match.</response>    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                string jwtSecret = _configuration["JWT:Secret"];
                byte[] jwtSecretBytes = Encoding.UTF8.GetBytes(jwtSecret);
                var authSigningKey = new SymmetricSecurityKey(jwtSecretBytes);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                return Ok(new ResponseWithToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized(new ResponseMsg { Message = "Invalid username or password!" });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/register
        ///     {
        ///         "email": "someUsername@mail.bg",
        ///         "password": "somePassword"
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with "Success" status and "User created successfully! message".</response>
        /// <response code="400">Returns "Bad Request" when user already exists or user creation failed.</response>    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return BadRequest(new ResponseMsg
                { Message = "User already exists!" });

            var hasher = new PasswordHasher<IdentityUser>();
            IdentityUser newUser = new IdentityUser()
            {
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            newUser.PasswordHash = hasher.HashPassword(newUser, model.Password);

            var result = await userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return BadRequest(new ResponseMsg
                { Message = "User creation failed! Please check user details and try again." });

            return Ok(new ResponseMsg { Message = "User created successfully!" });
        }
    }
}
