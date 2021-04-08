using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokolokoShop_User.Models;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PokolokoShop_User.Controllers
{
    [Route("account")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserInterfaces _user;

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("all")]
        public async Task<List<Account>> GetAllUser()
        {
            List<Account> accounts = await _user.GetAllUser();
            return await Task.Run(() => accounts);
        }
    }
}
