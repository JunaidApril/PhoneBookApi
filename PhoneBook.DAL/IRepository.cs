using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneBook.DAL
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
