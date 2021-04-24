using Repository.Domain;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUser : IGenereicInterface<Account>
    {
        public Task<Account> SignIn(string username, string password);

        public Task<List<Account>> GetAllUsers();

        public Task<Account> FindUserByUsername(string username);
        public Task<Account> FindUserByEmail(string email);
        public Task<Account> FindNewUser(DateTime createdDate);

        public Task<bool> DeleteUser(string username);
        public Task<bool> UpdateUser(Account acc);



        
    }
}
