using System.Collections.Generic;
using System.Data.Entity;

namespace Email_client.Model
{
    public class UsersInfoRepository : IRepository<UsersInfo>
    {
        UsersContext _db;
        public UsersInfoRepository(UsersContext context)
        {
            _db = context;
        }
        public void Create(UsersInfo user)
        {
            _db.UsersTable.Add(user); 
        }

        public void Delete(int id)
        {
            UsersInfo user = _db.UsersTable.Find(id);
            if (user != null)
            {
                _db.UsersTable.Remove(user);
            }
        }

        public UsersInfo Get(int id)
        {
            return _db.UsersTable.Find(id);
        }

        public IEnumerable<UsersInfo> GetAll()
        {
          return  _db.UsersTable;
        }

        public void Update(UsersInfo user)
        {
            _db.Entry(user).State = EntityState.Modified;
        }
    }
}
