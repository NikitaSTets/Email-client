using System;

namespace Email_client.Model
{
   public class UnitOfWork:IDisposable
    {
        UsersContext db = new UsersContext();
        UsersInfoRepository usersInfoRepository;

        public UsersInfoRepository Users
            {
                get
                 {
                if (usersInfoRepository == null)
                        usersInfoRepository = new UsersInfoRepository(db);
                    
                      return usersInfoRepository;
                 }   
            }

        public void Save()
        {
            db.SaveChanges();
        }
        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }      
    }
}
