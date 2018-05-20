using System;

namespace Email_client.Model
{
    public class UnitOfWork : IDisposable
    {
        UsersContext _db = new UsersContext();
        UsersInfoRepository _usersInfoRepository;

        public UsersInfoRepository Users
        {
            get
            {
                if (_usersInfoRepository == null)
                    _usersInfoRepository = new UsersInfoRepository(_db);

                return _usersInfoRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
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
