using Microsoft.EntityFrameworkCore;
using Repository.DataContext;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Functions
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(DatabaseContext context) : base(context)
        {

        }

        public ProductRepo() : base()
        {
            
        }


        public async override Task Delete(object ID)
        {
            Product p = await table.Where(p => p.ProductId.Equals(ID)).FirstOrDefaultAsync();
            p.DeleteDate = DateTime.Now;
        }

        public async override Task<List<Product>> GetAll()
        {
            List<Product> productList = new List<Product>();
            productList = await table.Where(p => p.DeleteDate == null && p.Status.Equals("Available")).ToListAsync();
            return productList;
        }

        public async override Task<Product> GetBy(object ID)
        {
            Product result = await table.FirstOrDefaultAsync(product => product.ProductId == (int) ID);
            return result;

        }

        public async Task<List<Product>> GetProductsByBrand(object ID)
        {
            List<Product> list = new List<Product>();
            list = await table.Where(product => product.BrandId == (int) ID).ToListAsync();
            return list;

        }

        public async Task<List<Product>> GetProductsByCate(object ID)
        {
            List<Product> list = new List<Product>();
            list = await table.Where(product => product.CategoryId == (int)ID).ToListAsync();
            return list;

        }

        public async override Task Update(Product obj)
        {
            Product p = await table.Where(p => p.ProductId.Equals(obj.ProductId)).FirstOrDefaultAsync();
            p.BrandId = obj.BrandId;
            p.CategoryId = obj.CategoryId;
            p.Description = obj.Description;
            p.Material = obj.Material;
            p.QuantityInStock = obj.QuantityInStock;
            p.Status = obj.Status;
            p.Price = obj.Price;
            p.ModifiedDate = DateTime.Now;
            table.Update(p);
            await Save();

        }

        public async Task<Product> GetNewestProduct(DateTime dateTime)
        {
            return await table.Where(pro => pro.CreatedDate.Equals(dateTime)).FirstOrDefaultAsync();
        }
    }
}
