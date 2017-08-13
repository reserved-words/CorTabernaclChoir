namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMessageFieldToLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Message");
        }
    }
}
