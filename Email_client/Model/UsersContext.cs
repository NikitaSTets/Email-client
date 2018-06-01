using System.Data.Entity;

namespace Email_client.Model
{
    public class UsersContext : DbContext
    {
      private const  string DbName="name=UsersDB";
        public UsersContext()
            : base(DbName)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersContext>());        
        }
         public virtual DbSet<UsersInfo> UsersTable { get; set; }
    }
}