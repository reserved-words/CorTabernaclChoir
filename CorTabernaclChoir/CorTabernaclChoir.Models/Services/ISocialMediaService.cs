using System.Collections.Generic;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Common.Services
{
    public interface ISocialMediaService
    {
        List<SocialMediaViewModel> GetAll();
        void Add(SocialMediaViewModel model, ImageFile logo);
        void Delete(int id);
        void Edit(SocialMediaViewModel model, ImageFile logo);
        SocialMediaViewModel Get(int id);
    }
}
