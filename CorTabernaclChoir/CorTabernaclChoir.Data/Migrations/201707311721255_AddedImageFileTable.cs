namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageFileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        File = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SocialMediaAccounts", "Name", c => c.String());
            AddColumn("dbo.SocialMediaAccounts", "ImageFileId", c => c.Int());
            CreateIndex("dbo.SocialMediaAccounts", "ImageFileId");
            AddForeignKey("dbo.SocialMediaAccounts", "ImageFileId", "dbo.ImageFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialMediaAccounts", "ImageFileId", "dbo.ImageFiles");
            DropIndex("dbo.SocialMediaAccounts", new[] { "ImageFileId" });
            DropColumn("dbo.SocialMediaAccounts", "ImageFileId");
            DropColumn("dbo.SocialMediaAccounts", "Name");
            DropTable("dbo.ImageFiles");
        }
    }
}
