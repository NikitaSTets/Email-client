namespace Email_client.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Email_client.Model.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Email_client.Model.UsersContext";
        }

        protected override void Seed(Email_client.Model.UsersContext context)
        {

        }
    }
}
