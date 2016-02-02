namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthTokens", "Appid", c => c.String());
            DropColumn("dbo.AuthTokens", "AuthClientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuthTokens", "AuthClientId", c => c.String());
            DropColumn("dbo.AuthTokens", "Appid");
        }
    }
}
