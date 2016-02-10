namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedStatusfromAuthTokens : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AuthTokens", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuthTokens", "Status", c => c.String());
        }
    }
}
