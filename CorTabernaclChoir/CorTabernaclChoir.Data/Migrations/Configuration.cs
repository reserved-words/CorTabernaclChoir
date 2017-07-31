using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CorTabernaclChoir.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Home.Any())
            {
                context.Home.Add(new Home
                {
                    Accompanist = "Accompanist Name",
                    MusicalDirector = "Musical Director Name",
                    MainText_E = "Main text in English",
                    MainText_W = "Main text in Welsh"
                });
            }

            if (!context.About.Any())
            {
                context.About.Add(new About
                {
                    AboutChoir_E = "Information about the choir in English",
                    AboutChoir_W = "Information about the choir in Welsh",
                    AboutMusicalDirector_E = "Information about the musical director in English",
                    AboutMusicalDirector_W = "Information about the musical director in Welsh",
                    AboutAccompanist_E = "Information about the accompanist in English",
                    AboutAccompanist_W = "Information about the accompanist in Welsh"
                });
            }

            if (!context.Contact.Any())
            {
                context.Contact.Add(new Contact
                {
                    MainText_E = "Contact information in English",
                    MainText_W = "Contact information in Welsh"
                });
            }

            if (!context.Join.Any())
            {
                context.Join.Add(new Join
                {
                    MainText_E = "Joining information in English",
                    MainText_W = "Joining information in Welsh"
                });
            }

            context.SaveChanges();
        }
    }
}
