namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationshipsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersAppsAccessGrantedExternalAuthClients",
                c => new
                    {
                        UsersAppsAccessGranted_Id = c.Int(nullable: false),
                        ExternalAuthClients_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UsersAppsAccessGranted_Id, t.ExternalAuthClients_Id })
                .ForeignKey("dbo.UsersAppsAccessGranteds", t => t.UsersAppsAccessGranted_Id, cascadeDelete: true)
                .ForeignKey("dbo.ExternalAuthClients", t => t.ExternalAuthClients_Id, cascadeDelete: true)
                .Index(t => t.UsersAppsAccessGranted_Id)
                .Index(t => t.ExternalAuthClients_Id);
            
            AddColumn("dbo.AuthTokens", "ExternalAuthClientsID", c => c.String(maxLength: 128));
            AddColumn("dbo.UsersAppsAccessGranteds", "ExternalAuthClientsID", c => c.String());
            CreateIndex("dbo.AuthTokens", "ExternalAuthClientsID");
            AddForeignKey("dbo.AuthTokens", "ExternalAuthClientsID", "dbo.ExternalAuthClients", "Id");
            DropColumn("dbo.AuthTokens", "Appid");
            DropColumn("dbo.UsersAppsAccessGranteds", "Appid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsersAppsAccessGranteds", "Appid", c => c.String());
            AddColumn("dbo.AuthTokens", "Appid", c => c.String());
            DropForeignKey("dbo.AuthTokens", "ExternalAuthClientsID", "dbo.ExternalAuthClients");
            DropForeignKey("dbo.UsersAppsAccessGrantedExternalAuthClients", "ExternalAuthClients_Id", "dbo.ExternalAuthClients");
            DropForeignKey("dbo.UsersAppsAccessGrantedExternalAuthClients", "UsersAppsAccessGranted_Id", "dbo.UsersAppsAccessGranteds");
            DropIndex("dbo.UsersAppsAccessGrantedExternalAuthClients", new[] { "ExternalAuthClients_Id" });
            DropIndex("dbo.UsersAppsAccessGrantedExternalAuthClients", new[] { "UsersAppsAccessGranted_Id" });
            DropIndex("dbo.AuthTokens", new[] { "ExternalAuthClientsID" });
            DropColumn("dbo.UsersAppsAccessGranteds", "ExternalAuthClientsID");
            DropColumn("dbo.AuthTokens", "ExternalAuthClientsID");
            DropTable("dbo.UsersAppsAccessGrantedExternalAuthClients");
        }
    }
}
