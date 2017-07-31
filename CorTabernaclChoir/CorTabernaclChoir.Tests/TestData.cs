using CorTabernaclChoir.Common.Models;
using System.Collections.Generic;

namespace CorTabernaclChoir.Tests
{
    public static class TestData
    {
        public static About About()
        {
            return new About
            {
                Id = 1,
                AboutAccompanist_E = "Acc in English",
                AboutAccompanist_W = "Acc in Welsh",
                AboutMusicalDirector_E = "Con in English",
                AboutMusicalDirector_W = "Con in Welsh",
                AboutChoir_E = "Main Text in English",
                AboutChoir_W = "Main Text in Welsh"
            };
        }

        public static Contact Contact()
        {
            return new Contact
            {
                Id = 1,
                MainText_E = "Main Text in English",
                MainText_W = "Main Text in Welsh"
            };
        }

        public static Home Home()
        {
            return new Home
            {
                Id = 1,
                Accompanist = "Acc",
                MusicalDirector = "Con",
                MainText_E = "Main Text in English",
                MainText_W = "Main Text in Welsh"
            };
        }

        public static List<GalleryImage> GalleryImages()
        {
            return new List<GalleryImage>
            {
                new GalleryImage
                {
                    Id = 1,
                    Caption_E = "Blah blah 1",
                    Caption_W = "Bla bla 1"
                },
                new GalleryImage
                {
                    Id = 2,
                    Caption_E = "Blah blah 2",
                    Caption_W = "Bla bla 2"
                },
                new GalleryImage
                {
                    Id = 3,
                    Caption_E = "Blah blah 3",
                    Caption_W = "Bla bla 3"
                },
                new GalleryImage
                {
                    Id = 4,
                    Caption_E = "Blah blah 4",
                    Caption_W = "Bla bla 4"
                },
                new GalleryImage
                {
                    Id = 5,
                    Caption_E = "Blah blah 5",
                    Caption_W = "Bla bla 5"
                }
            };
        }

        public static Join Join()
        {
            return new Join
            {
                Id = 1,
                MainText_E = "Main Text in English",
                MainText_W = "Main Text in Welsh",
                RehearsalTimes_E = "RT E",
                Location_E = "L E",
                Auditions_E = "A E",
                Concerts_E = "C E",
                Subscriptions_E = "S E",
                RehearsalTimes_W = "RT W",
                Location_W = "L W",
                Auditions_W = "A W",
                Concerts_W = "C W",
                Subscriptions_W = "S W"
            };
        }

        public static List<Post> Posts()
        {
            var list = new List<Post>();

            for (var i = 0; i < 25; i++)
            {
                list.Add(new Post
                {
                    Id = i + 1,
                    Published = new System.DateTime(2000, 1, 1, 14, 15, 2).AddDays(-1 * i),
                    Content_E = "Some content in English " + i,
                    Content_W = "Some content in Welsh " + i,
                    Title_E = "English Title " + i,
                    Title_W = "Welsh Title " + i,
                    Type = PostType.News,
                    PostImages = new List<PostImage> { new PostImage { Id = i + 1, PostId = i + 1 } }
                });
            }

            for (var i = 25; i < 50; i++)
            {
                list.Add(new Post
                {
                    Id = i + 1,
                    Published = new System.DateTime(2000, 1, 1, 14, 15, 2).AddDays(-1 * i),
                    Content_E = "Some content in English " + i,
                    Content_W = "Some content in Welsh " + i,
                    Title_E = "English Title " + i,
                    Title_W = "Welsh Title " + i,
                    Type = PostType.Visit
                });
            }

            return list;
        }

        public static List<Work> Works()
        {
            var list = new List<Work>();

            for (var i = 0; i < 25; i++)
            {
                list.Add(new Work
                {
                    Id = i + 1,
                    Year = (2000 + (i % 5)).ToString(),
                    Composer = "Someone Or Other " + i,
                    Title = "Something Or Other " + i
                });
            }

            return list;
        }

        public static List<Event> Events()
        {
            var list = new List<Event>();

            for (var i = 0; i < 25; i++)
            {
                list.Add(new Event
                {
                    Id = i + 1,
                    Date = new System.DateTime(2000, 1, 1, 14, 15, 2).AddDays(-1 * i),
                    Content_E = "Some content in English " + i,
                    Content_W = "Some content in Welsh " + i,
                    Title_E = "English Title " + i,
                    Title_W = "Welsh Title " + i,
                    Venue_E = "English Venue " + i,
                    Venue_W = "Welsh Venue " + i,
                    PostImages = new List<PostImage> { new PostImage { Id = i + 1, PostId = i + 1 } }
                });
            }

            return list;
        }

        public static List<SocialMediaAccount> SocialMediaAccounts()
        {
            var list = new List<SocialMediaAccount>();

            for (var i = 0; i < 25; i++)
            {
                list.Add(new SocialMediaAccount
                {
                    Id = i + 1,
                    Url = "URL " + (i + 1)
                });
            }

            return list;
        }
    }
}
