using System.Linq;
using AutoMapper;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Mapper.Profiles
{
    public class EnglishProfile : Profile
    {
        public EnglishProfile()
        {
            CreateMap<Home, HomeViewModel>()
                .ForMember(dest => dest.MainText, act => act.MapFrom(src => src.MainText_E));

            CreateMap<About, AboutViewModel>()
                .ForMember(dest => dest.AboutChoir, act => act.MapFrom(src => src.AboutChoir_E))
                .ForMember(dest => dest.AboutMusicalDirector, act => act.MapFrom(src => src.AboutMusicalDirector_E))
                .ForMember(dest => dest.AboutAccompanist, act => act.MapFrom(src => src.AboutAccompanist_E));

            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title_E))
                .ForMember(dest => dest.Venue, act => act.MapFrom(src => src.Venue_E))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Address_E))
                .ForMember(dest => dest.Images, act => act.MapFrom(src => src.PostImages.Select(im => im.Id).ToList()));

            CreateMap<GalleryImage, Image>()
                .ForMember(dest => dest.Caption,
                    act => act.MapFrom(src => string.Format(Resources.GalleryImageCaption, src.Caption_E, src.Year)));

            CreateMap<Join, JoinViewModel>()
                .ForMember(dest => dest.MainText, act => act.MapFrom(src => src.MainText_E))
                .ForMember(dest => dest.RehearsalTimes, act => act.MapFrom(src => src.RehearsalTimes_E))
                .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Location_E))
                .ForMember(dest => dest.Auditions, act => act.MapFrom(src => src.Auditions_E))
                .ForMember(dest => dest.Concerts, act => act.MapFrom(src => src.Concerts_E))
                .ForMember(dest => dest.Subscriptions, act => act.MapFrom(src => src.Subscriptions_E));

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title_E))
                .ForMember(dest => dest.Content, act => act.MapFrom(src => src.Content_E))
                .ForMember(dest => dest.Images, act => act.MapFrom(src => src.PostImages.ToList()));

            CreateMap<SocialMediaAccount, SocialMediaViewModel>();
        }
    }
}
