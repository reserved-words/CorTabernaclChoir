using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Common.Services
{
    public interface IPostsService
    {
        PostsViewModel Get(int page, PostType postType);
        PostViewModel Get(int id);
        Post GetForEdit(int id);
        void Save(Post model);
        void Delete(Post model);
    }
}
