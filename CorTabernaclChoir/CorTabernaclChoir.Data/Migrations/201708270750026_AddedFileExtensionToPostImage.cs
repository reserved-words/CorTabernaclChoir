namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFileExtensionToPostImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostImages", "FileExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostImages", "FileExtension");
        }
    }
}
