namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        TokenID = c.String(nullable: false, maxLength: 128),
                        Status = c.String(),
                        AuthClientId = c.String(),
                    })
                .PrimaryKey(t => t.TokenID);
            
            CreateTable(
                "dbo.ExternalAuthClients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExternalAuthClients");
            DropTable("dbo.AuthTokens");
        }
    }
}
