namespace ServerUsersOwinAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addednewfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExternalAuthClients", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExternalAuthClients", "Name");
        }
    }
}
