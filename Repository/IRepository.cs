using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IQueryable<T> GetItemsCollection();
        Task<T> GetItemAsync(int? id);
        void Create(T item);
        void Update(T itemToUpdate);
        Task DeleteAsync(int? id);
        Task SaveAsync();
        bool ItemExist(int id);

    }
}
