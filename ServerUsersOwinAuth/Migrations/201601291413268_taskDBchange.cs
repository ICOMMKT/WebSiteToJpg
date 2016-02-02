namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskDBchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersAppsAccessGranteds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Userid = c.String(),
                        Appid = c.String(),
                        AccessGranted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsersAppsAccessGranteds");
        }
    }
}
