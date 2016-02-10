namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnewfielAppsAccessGranted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersAppsAccessGranteds", "CreatedOn", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersAppsAccessGranteds", "CreatedOn");
        }
    }
}
