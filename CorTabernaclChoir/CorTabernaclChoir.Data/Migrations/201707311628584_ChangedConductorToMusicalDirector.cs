namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedConductorToMusicalDirector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.About", "AboutMusicalDirector_E", c => c.String(maxLength: 2000));
            AddColumn("dbo.About", "AboutMusicalDirector_W", c => c.String(maxLength: 2000));
            AddColumn("dbo.Home", "MusicalDirector", c => c.String(maxLength: 100));
            DropColumn("dbo.About", "AboutConductor_E");
            DropColumn("dbo.About", "AboutConductor_W");
            DropColumn("dbo.Home", "Conductor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Home", "Conductor", c => c.String(maxLength: 100));
            AddColumn("dbo.About", "AboutConductor_W", c => c.String(maxLength: 2000));
            AddColumn("dbo.About", "AboutConductor_E", c => c.String(maxLength: 2000));
            DropColumn("dbo.Home", "MusicalDirector");
            DropColumn("dbo.About", "AboutMusicalDirector_W");
            DropColumn("dbo.About", "AboutMusicalDirector_E");
        }
    }
}
