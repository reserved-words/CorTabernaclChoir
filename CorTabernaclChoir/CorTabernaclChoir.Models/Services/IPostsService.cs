using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Services
{
    public interface IPostsService
    {
        PostsViewModel Get(int page, PostType postType);
        Post GetForEdit(int id);
        void Save(Post model);
        void Delete(Post model);
    }
}
