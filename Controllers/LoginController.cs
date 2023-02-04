using Day3Paypal.Data;
using Day3Paypal.ViewModels;
using Day3Paypal.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;

namespace Day3Paypal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private IConfiguration _config;
        private IServiceProvider _serviceProvider;
        private ApplicationDbContext _db;

        public LoginController(SignInManager<IdentityUser> signInManager,
                                IConfiguration config,
                                IServiceProvider serviceProvider,
                                ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _config = config;
            _serviceProvider = serviceProvider;
            _db = context;
        }

        [HttpGet]
        [Route("List")]
        // Since we have cookie authentication and Jwt authentication we must
        // specify that we want Jwt authentication here.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
                                         , Roles = "Admin,Manager")]
        public IActionResult List()
        {
            var claim = HttpContext.User.Claims.ElementAt(0);
            string userName = claim.Value;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


            var techArray = new[] {
            new {Text = "Silverlight", Count = 10, Link="/Tags/Silverlight" },
            new {Text = "IIS 7", Count = 11, Link="http://iis.net" },
            new {Text = "IE 8", Count = 12, Link="/Tags/IE8" },
            new {Text = "C#", Count = 13, Link="/Tags/C#" },
            new {Text = "Azure", Count = 13, Link="?Tag=Azure" }
        };

            var responseObject = new
            {
                techArray = techArray
                                     ,
                userName = userName
                                     ,
                userId = userId
            };

            return new ObjectResult(responseObject);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email.ToUpper()
                                                                     , loginVM.Password
                                                                     , loginVM.RememberMe
                                                                     , lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var UserManager = _serviceProvider
                                         .GetRequiredService<UserManager<IdentityUser>>();

                    var user = await UserManager.FindByEmailAsync(loginVM.Email);

                    if (user != null)
                    {
                        var tokenString = GenerateJSONWebToken(user);

                        var firstName = (from u in _db.MyRegisteredUsers
                                          where u.Email == loginVM.Email
                                          select u.FirstName).FirstOrDefault();

                        var userRoles = _db.UserRoles
                                .Where(ur => ur.UserId == user.Id)
                                .Select(ur => new { ur.RoleId }).ToList();

                        var jsonOK = new { firstName = firstName, tokenString = tokenString, roles = userRoles,
                                           StatusCode = "OK" };

                        return new ObjectResult(jsonOK);
                    }
                }
                else if (result.IsLockedOut)
                {
                    var jsonLocked = new { tokenString = "", StatusCode = "Locked out." };
                    return new ObjectResult(jsonLocked);
                }
            }
            var jsonInvalid = new { tokenString = "", StatusCode = "Invalid Login." };
            return new ObjectResult(jsonInvalid);
        }

        List<Claim> AddUserRoleClaims(List<Claim> claims, string userId)
        {
            // Get current user's roles. 
            var userRoleList = _db.UserRoles.Where(ur => ur.UserId == userId);
            var roleList = from ur in userRoleList
                           from r in _db.Roles
                           where r.Id == ur.RoleId
                           select new { r.Name };

            // Add each of the user's roles to the claims list.
            foreach (var roleItem in roleList)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleItem.Name));
            }
            return claims;
        }

        string GenerateJSONWebToken(IdentityUser user)
        {
            var securityKey
                = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials
                = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

            claims = AddUserRoleClaims(claims, user.Id);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
                                         , Roles = "Manager")]
        [Route("UserDetails")]
        [HttpPost]
        public object UserDetails([FromBody] NamesVM names)
        {
            var claim = HttpContext.User.Claims.ElementAt(0);
            string userName = claim.Value;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            object userDetails = new
            {
                firstName = names.FirstName,
                lastName = names.LastName,
                userId = userId
            };

            return new ObjectResult(userDetails);

        }

        [HttpGet]
        [Route("Csrf")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
                                         , Roles = "Manager")]
        [Authorize(Roles = "Admin")]
        public IActionResult Csrf()
        {
            return new ObjectResult(new
            {
                email = this.User.Identities.ElementAt(0).Name,
                userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier)
            });
        }
    }
}
