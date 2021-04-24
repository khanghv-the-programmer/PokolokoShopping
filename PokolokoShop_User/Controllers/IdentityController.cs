using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokolokoShop_User.Models;
using PokolokoShop_User.Options;
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


namespace PokolokoShop_User.Controllers
{
    [Route("api/identity")]
    [ApiController]
    
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserInterfaces _userService;
        private readonly JWTSettings _jwtSttings;
        IHostingEnvironment _env;
        public IdentityController(IIdentityService identityService, IUserInterfaces userSevice, JWTSettings jWTSettings, IHostingEnvironment env)
        {
            _identityService = identityService;
            _userService = userSevice;
            _jwtSttings = jWTSettings;
            _env = env;
        }
        [Route("test")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Reesting([FromBody] string user)
        {
            Console.WriteLine("Input: " + user);
            var body = HttpContext.Request;
            var bodyStr = "";
            using (StreamReader reader
                      = new StreamReader(body.Body, Encoding.UTF8, true, 1024, true))
            {

                bodyStr = await reader.ReadToEndAsync();
                Console.WriteLine(bodyStr);
            }
            return await Task.Run(() => Ok());
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Account user)
        {


            if (user == null)
            {
                Console.WriteLine("Vãi l luôn đầu cắt moi");
            }
            Console.WriteLine(user.Username);
            user.RoleId = 2;
            var registrationResult = await _userService.RegisterAsync(user);
            if (registrationResult.Success)
            {
                return await Task.Run(() => Ok(user));
            }

            return await Task.Run(() => BadRequest(registrationResult));

        }
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
