namespace GetWebSitesToJPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageCropDatas",
                c => new
                    {
                        ImageID = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        ContainerWidth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImageCropDatas");
        }
    }
}
