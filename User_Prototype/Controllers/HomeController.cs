using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using User_Prototype.Models;

namespace User_Prototype.Controllers
{
    [Route("user")]
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserInterfaces _user;

        public HomeController(ILogger<HomeController> logger, IUserInterfaces user)
        {
            _logger = logger;
            _user = user;
        }
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("loginfacebook")]
        public async Task<IActionResult> Index([FromBody] UserFacebookRequest req)
        {
            var respond = await _user.LoginWithFacebook(req.AccessToken);
            if(respond == null)
            {
                return await Task.Run(() => BadRequest(new AuthenticationResult
                {
                    Errors = new[] { "Invalid !" }
                }));
            }
            if(!respond.Success)
            {
                return await Task.Run(() => Ok(new AuthenticationResult
                {
                    NewUser = respond.NewUser
                }));

            }
            return await Task.Run(() => Ok(new AuthenticationResult
            {
                Token = respond.Token,
                RefreshToken = respond.RefreshToken,
            }));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
