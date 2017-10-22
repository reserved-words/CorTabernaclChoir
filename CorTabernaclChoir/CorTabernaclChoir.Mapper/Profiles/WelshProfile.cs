using System.Linq;
using AutoMapper;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Mapper.Profiles
{
    public class WelshProfile : Profile
    {
        public WelshProfile()
        {
            CreateMap<Home, HomeViewModel>()
                .ForMember(dest => dest.MainText, act => act.MapFrom(src => src.MainText_W));

            CreateMap<About, AboutViewModel>()
                .ForMember(dest => dest.AboutChoir, act => act.MapFrom(src => src.AboutChoir_W))
                .ForMember(dest => dest.AboutMusicalDirector, act => act.MapFrom(src => src.AboutMusicalDirector_W))
                .ForMember(dest => dest.AboutAccompanist, act => act.MapFrom(src => src.AboutAccompanist_W));

            CreateMap<GalleryImage, Image>()
                .ForMember(dest => dest.Caption,
                    act => act.MapFrom(src => string.Format(Resources.GalleryImageCaption, src.Caption_W, src.Year)));

            CreateMap<Join, JoinViewModel>()
                .ForMember(dest => dest.MainText, act => act.MapFrom(src => src.MainText_W))
                .ForMember(dest => dest.RehearsalTimes, act => act.MapFrom(src => src.RehearsalTimes_W))
                .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Location_W))
                .ForMember(dest => dest.Auditions, act => act.MapFrom(src => src.Auditions_W))
                .ForMember(dest => dest.Concerts, act => act.MapFrom(src => src.Concerts_W))
                .ForMember(dest => dest.Subscriptions, act => act.MapFrom(src => src.Subscriptions_W));

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title_W))
                .ForMember(dest => dest.Content, act => act.MapFrom(src => src.Content_W))
                .ForMember(dest => dest.Images, act => act.MapFrom(src => src.PostImages.ToList()));

            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title_W))
                .ForMember(dest => dest.Venue, act => act.MapFrom(src => src.Venue_W))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Address_W))
                .ForMember(dest => dest.Details, act => act.MapFrom(src => src.Content_W))
                .ForMember(dest => dest.Images, act => act.MapFrom(src => src.PostImages.ToList()));

            CreateMap<Event, EventSummaryViewModel>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title_W))
                .ForMember(dest => dest.Venue, act => act.MapFrom(src => src.Venue_W))
                .ForMember(dest => dest.DateTime, act => act.MapFrom(src => string.Format(EventDateTimeFormat, src.Date)));

            CreateMap<Post, PostSummaryViewModel>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title_W));

            CreateMap<SocialMediaAccount, SocialMediaViewModel>();
        }
    }
}
