using Repository.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IToken : IGenereicInterface<RefreshToken>
    {

        public Task<RefreshToken> GetRefreshToken(Guid token);
    }
}
