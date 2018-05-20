using System.Data.Entity;
namespace Email_client.Model
{
    

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("name=UsersDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersContext>());
          
        }
         public virtual DbSet<UsersInfo> UsersTable { get; set; }
    }
}