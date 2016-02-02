namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesAutTokensDB : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AuthTokens");
            AddColumn("dbo.AuthTokens", "Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AuthTokens", "Token", c => c.String());
            AddPrimaryKey("dbo.AuthTokens", "Id");
            DropColumn("dbo.AuthTokens", "TokenID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuthTokens", "TokenID", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.AuthTokens");
            DropColumn("dbo.AuthTokens", "Token");
            DropColumn("dbo.AuthTokens", "Id");
            AddPrimaryKey("dbo.AuthTokens", "TokenID");
        }
    }
}
