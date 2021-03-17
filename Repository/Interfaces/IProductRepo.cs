using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductRepo : IGenereicInterface<Product>
    {
        public Task<List<Product>> GetProductsByBrand(object ID);
        public Task<List<Product>> GetProductsByCate(object ID);
        public Task<Product> GetNewestProduct(DateTime dateTime);
    }
}
