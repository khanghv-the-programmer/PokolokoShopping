using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Prototype.Models
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
