namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedContentTypeToImageFileTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageFiles", "ContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageFiles", "ContentType");
        }
    }
}
