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
        DatabaseContext context = null;
        GenericRepo<Product> productRepo = null;
        GenericRepo<Image> imageRepo = null;
        GenericRepo<Category> cateRepo = null;
        GenericRepo<Brand> brandRepo = null;

        public ProductFunctions()
        {
            context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            productRepo = new GenericRepo<Product>(context);
            imageRepo = new GenericRepo<Image>(context); ;
            cateRepo = new GenericRepo<Category>(context);
            brandRepo = new GenericRepo<Brand>(context);

        }

        public async Task<Image> AddImage(List<Image> img)
        {
            
            try
            {
                foreach(Image i in img)
                {
                    await imageRepo.Add(i);
                    await imageRepo.Save();
                }
                
                return img.ElementAt(0);
            }
            catch(Exception)
            {
                return null;
            }

        }

        public async Task<Product> AddProduct(Product p)
        {

            
                await context.Product.AddAsync(p);
                await context.SaveChangesAsync();
            
            return await context.Product.Where(pro => pro.CreatedDate.Equals(p.CreatedDate)).FirstOrDefaultAsync();
        }

        public async Task<Image> AddThumbnailImg(Image thumb)
        {

            await context.Image.AddAsync(thumb);
            await context.SaveChangesAsync();
            return await context.Image.Where(img => img.CreatedDate.Equals(thumb.CreatedDate)).FirstOrDefaultAsync();
        }

        public async Task<bool> EditProduct(Product editedP)
        {

            Product p = await GetProductById(editedP.ProductId);
            p.BrandId = editedP.BrandId;
            p.CategoryId = editedP.CategoryId;
            p.Description = editedP.Description;
            p.Material = editedP.Material;
            p.QuantityInStock = editedP.QuantityInStock;
            p.Status = editedP.Status;
            p.Price = editedP.Price;
            p.ModifiedDate = DateTime.Now;
            try
            {
                context.Product.Update(p);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }



        public async Task<Boolean> EditThumbnail(Image thumbnail)
        {

            Image thumb = await GetThumbnail(thumbnail.ProductId);
            thumb.Image1 = thumbnail.Image1;
            thumb.ModifiedDate = DateTime.Now;
            thumb.Name = thumbnail.Name;
            try
            {
                context.Update(thumb);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Brand> FindBrandById(int id)
        {
            Brand brand = null;
            brand = await context.Brand.Where(brand => brand.BrandId == id).FirstOrDefaultAsync();
            return brand;
        }


        public async Task<Category> FindCateById(int id)
        {
            Category cate = null;
            cate = await context.Category.Where(categ => categ.CategoryId == id).FirstOrDefaultAsync();
            return cate;

            
        }

        public async Task<List<Product>> FindProductByBrand(int? id)
        {
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
            List<Brand> brandList = await context.Brand.ToListAsync();
            return brandList;
        }

        public async Task<List<Category>> GetCategoryList()
        {
            List<Category> cateList = await context.Category.ToListAsync();
            return cateList;
        }

        public async Task<Product> GetProductById(int id)
        {
            Product result = await context.Product.FirstOrDefaultAsync(product => product.ProductId == id);
            result.Image = await context.Image.Where(imgList => imgList.ProductId == id).ToListAsync();
            result.Brand = await FindBrandById(result.BrandId);
            result.Category = await FindCateById(result.CategoryId);
            
            
            return result;
        }

        public async Task<List<Product>> GetProductList()
        {
            List<Product> productList = new List<Product>();
            Brand brand = new Brand();
            Category cate = new Category();
            List<Image> imgList = new List<Image>();
            productList = await context.Product.Where(p => p.DeleteDate == null).ToListAsync();
        
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

        public async Task<Image> GetThumbnail(int productID)
        {
            Image thumb = new Image();
            thumb = await context.Image.Where(img => img.ProductId == productID).FirstOrDefaultAsync();
            return thumb;

        }
    }
}
