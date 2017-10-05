using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Common.Services
{
    public interface IPostsService
    {
        PostsViewModel Get(int page, PostType postType);
        PostViewModel Get(int id);
        EditPostViewModel GetForEdit(int id);
        int Save(EditPostViewModel model);
        int SaveImage(int postId, string fileExtension);
        void Delete(Post model);
        void DeleteImage(int testImageId);
    }
}
