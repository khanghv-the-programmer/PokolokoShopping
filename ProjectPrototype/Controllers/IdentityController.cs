    using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokolokoShop_User.Models;
using PokolokoShop_User.Options;
using Repository.Domain;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace PokolokoShop_User.Controllers
{
    [Route("identity")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IUserInterfaces _userService;
        private readonly JWTSettings _jwtSttings;
        public IdentityController(IIdentityService identityService, IUserInterfaces userSevice, JWTSettings jWTSettings)
        {
            _identityService = identityService;
            _userService = userSevice;
            _jwtSttings = jWTSettings;
        }
        [Route("test")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Reesting([FromBody] string user)
        {
            Console.WriteLine("Input: " + user);
            return await Task.Run(() => Ok());
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Account user)
        {
            var body = HttpContext.Request.Body.ToString();
            Console.WriteLine(body);
            if(user == null)
            {
                Console.WriteLine("Vãi l luôn đầu cắt moi");
            }
            Console.WriteLine(user.Username);
            user.RoleId = 2;
            var registrationResult = await _userService.RegisterAsync(user);
            if(registrationResult != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSttings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, registrationResult.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, registrationResult.Email),
                    new Claim("Username", registrationResult.Username),

                }),
                    Expires = DateTime.Now.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return await Task.Run(() => Ok(new AuthenticationResult
                {
                    Success = true,
                    Token = tokenHandler.WriteToken(token)
                }));
            }else
            {
                return await Task.Run(() => BadRequest(new AuthenticationResult
                {
                    Errors = new []
                    {
                        "Can not register right now!",
                        "Try again!",
                    }
                }));
            }
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
