using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Prototype.Models
{
    public class RequestLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
