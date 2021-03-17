using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenereicInterface<T> where T:class
    {
        Task<List<T>> GetAll();
        Task<T> GetBy(object ID);

        Task Add(T obj);
        Task Update(T obj);
        Task Delete(object ID);
        Task Save();
        
    }
}
