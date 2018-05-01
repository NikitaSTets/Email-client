namespace Email_client.Model
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("name=UsersDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersContext>());
            //UsersTable.Add(new UsersInfo() { Login = "nikitstets@gmail.com",Password ="StackCorporation"});
        }
         public virtual DbSet<UsersInfo> UsersTable { get; set; }
    }
}