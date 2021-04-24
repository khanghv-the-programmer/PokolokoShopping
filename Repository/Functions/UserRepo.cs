using Microsoft.EntityFrameworkCore;
using Repository.DataContext;
using Repository.Domain;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Functions
{
    public class UserRepo : GenericRepo<Account>, IUser
    {
        public UserRepo(DatabaseContext context) : base(context)
        {

        }
        public UserRepo(): base()
        {
            
        }
        public async Task<Account> SignIn(string username, string password)
        {
            Account user = await table.Where(user => user.Username.Equals(username) && user.Password.Equals(password) && user.Status.Equals("Available")).FirstOrDefaultAsync();
            return user;
        }

        

        public async Task<List<Account>> GetAllUsers()
        {
            List<Account> peopleList = new List<Account>();
            peopleList = await table.Where(p => p.Status.Equals("Available")).ToListAsync();
            return peopleList;
        }

        public async Task<Account> FindUserByUsername(string username)
        {
            Account user = await table.Where(user => user.Username.Equals(username)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Account> FindNewUser(DateTime createdDate)
        {
            Account user = await table.Where(user => user.CreatedDate.Equals(createdDate)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> DeleteUser(string username)
        {
            try
            {
                Account acc = await table.Where(acc => acc.Username.Equals(username)).FirstOrDefaultAsync();
                acc.Status = "Deleted";
                table.Update(acc);
                await Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(Account acc)
        {
            try
            {
                Account existed = await table.Where(acc => acc.Username.Equals(acc.Username)).FirstOrDefaultAsync();
                existed.Password = acc.Password;
                existed.FullName = acc.FullName;
                existed.PhoneNumber = acc.PhoneNumber;
                existed.ModifiedDate = DateTime.Now;
                existed.Email = acc.Email;
                existed.DateOfBirth = acc.DateOfBirth;
                table.Update(existed);
                await Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Account> FindUserByEmail(string email)
        {
            Account user = await table.Where(user => user.Email.Equals(email)).FirstOrDefaultAsync();
            return user;
        }
    }
}
