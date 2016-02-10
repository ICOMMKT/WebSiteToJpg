namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiedNewField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UsersAppsAccessGranteds", "CreatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsersAppsAccessGranteds", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
