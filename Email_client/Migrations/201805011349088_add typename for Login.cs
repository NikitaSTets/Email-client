namespace Email_client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtypenameforLogin : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UsersInfoes");
            AlterColumn("dbo.UsersInfoes", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.UsersInfoes", "Login", c => c.String(maxLength: 450, unicode: false));
            AddPrimaryKey("dbo.UsersInfoes", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UsersInfoes");
            AlterColumn("dbo.UsersInfoes", "Login", c => c.String(nullable: false, maxLength: 450, unicode: false));
            AlterColumn("dbo.UsersInfoes", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.UsersInfoes", "Id");
        }
    }
}
