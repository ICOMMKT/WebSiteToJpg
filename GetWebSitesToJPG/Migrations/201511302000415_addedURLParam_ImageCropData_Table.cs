namespace GetWebSitesToJPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedURLParam_ImageCropData_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageCropDatas", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageCropDatas", "Url");
        }
    }
}
