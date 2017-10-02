using AutoMapper;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Mapper.Profiles
{
    public class PostsProfile : Profile
    {
        public PostsProfile()
        {
            CreateMap<PostImage, PostImageViewModel>();

            CreateMap<Post, EditPostViewModel>()
                .ForMember(dest => dest.PostImages, act => act.MapFrom(src => src.PostImages));

            CreateMap<PostImageViewModel, PostImage>();

            CreateMap<EditPostViewModel, Post>();
        }
    }
}
