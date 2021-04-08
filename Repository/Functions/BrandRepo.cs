using Microsoft.EntityFrameworkCore;
using Repository.DataContext;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Functions
{
    public class BrandRepo : GenericRepo<Brand>
    {
        public BrandRepo(DatabaseContext context) : base(context)
        {
        }

        public BrandRepo() : base()
        {

        }

        public async override Task<List<Brand>> GetAll()
        {
            List<Brand> list = await table.ToListAsync();
            return list;
        }

        public async override Task<Brand> GetBy(object ID)
        {
            return await table.Where(brand => brand.BrandId == (int)ID).FirstOrDefaultAsync();
        }

        
    }
}
