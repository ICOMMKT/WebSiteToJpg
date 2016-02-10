namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedAuthTokensTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuthTokens", "ExternalAuthClientsID", "dbo.ExternalAuthClients");
            DropIndex("dbo.AuthTokens", new[] { "ExternalAuthClientsID" });
            AddColumn("dbo.UsersAppsAccessGranteds", "Token", c => c.String());
            AddColumn("dbo.UsersAppsAccessGranteds", "Key", c => c.String());
            DropTable("dbo.AuthTokens");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Token = c.String(),
                        ExternalAuthClientsID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.UsersAppsAccessGranteds", "Key");
            DropColumn("dbo.UsersAppsAccessGranteds", "Token");
            CreateIndex("dbo.AuthTokens", "ExternalAuthClientsID");
            AddForeignKey("dbo.AuthTokens", "ExternalAuthClientsID", "dbo.ExternalAuthClients", "Id");
        }
    }
}
