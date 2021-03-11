using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IProduct
    {
        Task<Product> AddProduct(Product p);
        Task<List<Product>> GetProductList();

        Task<List<Category>> GetCategoryList();
        Task<List<Brand>> GetBrandList();
        Task<List<Product>> FindProductByCategory(int? id);
        
    }
}
