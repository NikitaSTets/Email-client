using System.Collections.Generic;

namespace Email_client.Model
{
    interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}