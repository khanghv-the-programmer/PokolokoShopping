using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Repository.Interfaces;
using Repository.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Repository.Functions
{
    public class ProductFunctions : IProduct
    {

        public async Task<Product> AddProduct(Product p)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                await context.Product.AddAsync(p);
                await context.SaveChangesAsync();
            }
            return p;
        }

        

        public async Task<List<Product>> FindProductByCategory(int? id)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            List<Image> imgList = new List<Image>();
            List<Product> proList = await context.Product.Where(pro => pro.CategoryId == id).ToListAsync();
            foreach (Product product in proList)
            {
                product.Image = await context.Image.Where(imgList => imgList.ProductId == product.ProductId).ToListAsync();
            }
            return proList;
        }

        public async Task<List<Brand>> GetBrandList()
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            List<Brand> brandList = await context.Brand.ToListAsync();
            return brandList;
        }

        public async Task<List<Category>> GetCategoryList()
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            List<Category> cateList = await context.Category.ToListAsync();
            return cateList;
        }

        public async Task<List<Product>> GetProductList()
        {
            List<Product> productList = new List<Product>();
            Brand brand = new Brand();
            Category cate = new Category();
            List<Image> imgList = new List<Image>();
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            productList = await context.Product.ToListAsync();
        
            foreach (Product product in productList)
            {
                

                    brand = await context.Brand.FirstOrDefaultAsync(brand => brand.BrandId == product.BrandId);
                    cate = await context.Category.FirstOrDefaultAsync(cate => cate.CategoryId == product.CategoryId);
                    imgList = await context.Image.Where(imgList => imgList.ProductId == product.ProductId).ToListAsync();
                    product.Brand = brand;
                    product.Category = cate;
                    



            }
            return productList;
        }
    }
}
