namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedContactEmailsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactEmails");
        }
    }
}
