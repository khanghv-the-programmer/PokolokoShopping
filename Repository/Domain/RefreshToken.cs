using Repository.Entities;
using System;
using System.Collections.Generic;

namespace Repository.Domain
{
    public partial class RefreshToken
    {
        public Guid Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool? IsUsed { get; set; }
        public bool? IsValidated { get; set; }
        public string Username { get; set; }

        public virtual Account UsernameNavigation { get; set; }
    }
}
