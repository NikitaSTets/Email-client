using System.Collections.Generic;
using System.Data.Entity;

namespace Email_client.Model
{
    public class UsersInfoRepository : IRepository<UsersInfo>
    {
        UsersContext db;
        public UsersInfoRepository(UsersContext context)
        {
            db = context;
        }
        public void Create(UsersInfo user)
        {
            db.UsersTable.Add(user); 
        }

        public void Delete(int id)
        {
            UsersInfo user = db.UsersTable.Find(id);
            if (user != null)
            {
                db.UsersTable.Remove(user);
            }
        }

        public UsersInfo Get(int id)
        {
            return db.UsersTable.Find(id);
        }

        public IEnumerable<UsersInfo> GetAll()
        {
          return  db.UsersTable;
        }

        public void Update(UsersInfo user)
        {
            db.Entry(user).State = EntityState.Modified;
        }
    }
}
