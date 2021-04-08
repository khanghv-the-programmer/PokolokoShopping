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
    public class CategoryRepo : GenericRepo<Category>
    {
        public CategoryRepo(DatabaseContext context) : base(context)
        {
        }
        public CategoryRepo() : base()
        {

        }

        public async override Task<List<Category>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async override Task<Category> GetBy(object ID)
        {
            return await table.Where(cate => cate.CategoryId == (int) ID).FirstOrDefaultAsync();
        }
    }
}
