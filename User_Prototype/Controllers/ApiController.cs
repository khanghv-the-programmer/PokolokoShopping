using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Domain;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User_Prototype.Models;

namespace User_Prototype.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class ApiController : ControllerBase
    {
        
        private readonly IUserInterfaces _userService;
        private readonly Logic.Options.JWTSettings _jwtSttings;
        IHostingEnvironment _env;
        public ApiController( IUserInterfaces userSevice, Logic.Options.JWTSettings jWTSettings, IHostingEnvironment env)
        {
           
            _userService = userSevice;
            _env = env;
            _jwtSttings = jWTSettings;
        }




        [Route("test")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reesting([FromBody] TestObject user)
        {
            Console.WriteLine("Input: " + user.Name);
            var body = HttpContext.Request;
            var bodyStr = "";
            using (StreamReader reader
                      = new StreamReader(body.Body, Encoding.UTF8, true, 1024, true))
            {

                bodyStr = await reader.ReadToEndAsync();
                Console.WriteLine(bodyStr);
            }
            return await Task.Run(() => Ok(user));
        }
        [Route("getkey")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate()
        {
            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie"),
            };
            var secretBytes = Encoding.UTF8.GetBytes(_jwtSttings.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                null,
                null,
                claim,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials);
            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return await Task.Run(() => Ok(new { access_token = tokenJson }));
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Account user)
        {
            if (ModelState.IsValid)
            {
                user.RoleId = 2;
                var registrationResult = await _userService.RegisterAsync(user);
                if (registrationResult.Success)
                {
                    return await Task.Run(() => Ok(user));
                }
            }


            return await Task.Run(() => BadRequest(new AuthenticationResult
            {
                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage)),
            }));

                
        }
        public IActionResult Index()
        {
            return Ok();
        }



        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] RequestLogin req)
        {

            AuthenticationResult result = await _userService.SignIn(req.Username, req.Password);

            
            if(result.Success)
            {
                
                return await Task.Run(() => Ok(result));
            }else
            {
                return await Task.Run(() => BadRequest(result));
            }
            
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest req)
        {

            AuthenticationResult result = await _userService.RefreshTokenAsync(req.Token, req.RefreshToken );


            if (result.Success)
            {

                return await Task.Run(() => Ok(result));
            }
            else
            {
                return await Task.Run(() => BadRequest(result));
            }

        }

    }
}
