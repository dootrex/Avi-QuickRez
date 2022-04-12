using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using rolesDemoSSD.Data;
using rolesDemoSSD.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace rolesDemoSSD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private IConfiguration _config;
        private IServiceProvider _serviceProvider;
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public LoginController(SignInManager<IdentityUser> signInManager,
                                IConfiguration config,
                                IServiceProvider serviceProvider,
                                ApplicationDbContext context,
                                UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _serviceProvider = serviceProvider;
            _context = context;
        }

        [HttpGet]
        [Route("List")]
        // Since we have cookie authentication and Jwt authentication we must
        // specify that we want Jwt authentication here.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Manager")]
        public IActionResult List()
        {
            var claim = HttpContext.User.Claims.ElementAt(0);
            string userName = claim.Value;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var customUser = _context.MyRegisteredUsers.Where(mru => mru.Email == userName).FirstOrDefault();


            var techArray = new[] {
            new {Text = "Silverlight", Count = 10, Link="/Tags/Silverlight" },
            new {Text = "IIS 7", Count = 11, Link="http://iis.net" },
            new {Text = "IE 8", Count = 12, Link="/Tags/IE8" },
            new {Text = "C#", Count = 13, Link="/Tags/C#" },
            new {Text = "Azure", Count = 13, Link="?Tag=Azure" }
        };

            var responseObject = new
            {
                techArray = techArray,
                userName = userName,
                userId = userId,
                //firstName = customUser.FirstName
            };
            return new ObjectResult(responseObject);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync([FromBody] LoginVM loginVM)
        {
            //System.Threading.Thread.Sleep(2000);
            if (ModelState.IsValid)
            {
                var result = await
                            _signInManager.PasswordSignInAsync(loginVM.Email.ToUpper(),
                            loginVM.Password, loginVM.RememberMe, lockoutOnFailure: true);


                if (result.Succeeded)
                {
                    var UserManager = _serviceProvider
                        .GetRequiredService<UserManager<IdentityUser>>();
                    var user = await UserManager.FindByEmailAsync(loginVM.Email);

                    //if (user != null)
                    //{
                    //    var tokenString = GenerateJSONWebToken(user);
                    //    var customUser = _context.MyRegisteredUsers.Where(mru => mru.Email == loginVM.Email).FirstOrDefault();
                    //    var jsonOK = new { tokenString = tokenString, StatusCode = "OK", Name = customUser.FirstName };

                    //    return new ObjectResult(jsonOK);
                    //}
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



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")] //only Admin role can access this method
        [HttpPost]
        [Route("name")] //https://localhost:44392/api/login/name
        public async Task<IActionResult> Name([FromBody] nameVM nameParam)
        {
            var claim = HttpContext.User.Claims.ElementAt(0);
            string userName = claim.Value;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // get ID from jwt
            var fName = nameParam.firstName;  // get first and last name from front end. 
            var lName = nameParam.lastName;  // get first and last name from front end. 

            var nameAndID = new { firstName = fName, lastName = lName, userId = userId }; // return first last and userID. 
            return new ObjectResult(nameAndID);
        }




        List<Claim> AddUserRoleClaims(List<Claim> claims, string userId)
        {
            // Get current user's roles. 
            var userRoleList = _context.UserRoles.Where(ur => ur.UserId == userId);
            var roleList = from ur in userRoleList
                           from r in _context.Roles
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







        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                var jsonResponse =
                    new { message = "User credentials are invalid. Please try again." };
                return new ObjectResult(jsonResponse);
            }

            var user = new IdentityUser
            {
                UserName = registerVM.Email,
                Email = registerVM.Email
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            //if (result.Succeeded)
            //{
            //    // Normally this code would be placed in a repository.
            //    MyRegisteredUser registerUser = new MyRegisteredUser()
            //    {
            //        Email = registerVM.Email,
            //        FirstName = registerVM.FirstName
            //    };
            //    _context.MyRegisteredUsers.Add(registerUser);
            //    _context.SaveChanges();
            //}
            var jsonOK = new
            {
                message = "The user " + registerVM.Email + " has been created.",
                StatusCode = "OK"
            };
            return new ObjectResult(jsonOK);
        }

    }

}
