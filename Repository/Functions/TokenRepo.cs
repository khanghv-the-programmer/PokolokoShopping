using Microsoft.EntityFrameworkCore;
using Repository.DataContext;
using Repository.Domain;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Functions
{
    public class TokenRepo : GenericRepo<RefreshToken>, IToken
    {
        public TokenRepo() : base ()
        {
                
        }

        public TokenRepo(DatabaseContext context) : base(context)
        {
                
        }
        public async Task<RefreshToken> GetRefreshToken(Guid token)
        {
            return await table.SingleOrDefaultAsync(x => x.Token.Equals(token));
        }

        public async override Task Update(RefreshToken obj)
        {
            RefreshToken p = new RefreshToken
            {   
                
                IsUsed = obj.IsUsed,
            };
            table.Update(p);
            await Save();

        }
    }
}
