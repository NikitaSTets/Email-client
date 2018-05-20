using System;

namespace Email_client.Model
{
    public class UnitOfWork : IDisposable
    {
        UsersContext db = new UsersContext();
        UsersInfoRepository _usersInfoRepository;

        public UsersInfoRepository Users
        {
            get
            {
                if (_usersInfoRepository == null)
                    _usersInfoRepository = new UsersInfoRepository(db);

                return _usersInfoRepository;
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
