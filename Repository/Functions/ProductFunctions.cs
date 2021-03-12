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

        public async Task<Brand> FindBrandById(int id)
        {
            Brand brand = null;
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            brand = await context.Brand.Where(brand => brand.BrandId == id).FirstOrDefaultAsync();
            return brand;
        }


        public async Task<Category> FindCateById(int id)
        {
            Category cate = null;
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            cate = await context.Category.Where(categ => categ.CategoryId == id).FirstOrDefaultAsync();
            return cate;

            
        }

        public async Task<List<Product>> FindProductByBrand(int? id)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            List<Image> imgList = new List<Image>();
            List<Product> proList = await context.Product.Where(pro => pro.BrandId == id).ToListAsync();
            foreach (Product product in proList)
            {
                product.Image = await context.Image.Where(imgList => imgList.ProductId == product.ProductId).ToListAsync();
                product.Brand = await FindBrandById(id.GetValueOrDefault());
            }
            return proList;
        }

        public async Task<List<Product>> FindProductByCategory(int? id)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            List<Image> imgList = new List<Image>();
            List<Product> proList = await context.Product.Where(pro => pro.CategoryId == id).ToListAsync();
            foreach (Product product in proList)
            {
                product.Image = await context.Image.Where(imgList => imgList.ProductId == product.ProductId).ToListAsync();
                product.Category = await FindCateById(id.GetValueOrDefault());
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

        public async Task<Product> GetProductById(int id)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            Product result = await context.Product.FirstOrDefaultAsync(product => product.ProductId == id);
            result.Image = await context.Image.Where(imgList => imgList.ProductId == result.ProductId).ToListAsync();
            
            return result;
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
