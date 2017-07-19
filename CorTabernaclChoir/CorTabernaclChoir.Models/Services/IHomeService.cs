using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Common.Services
{
    public interface IHomeService
    {
        HomeViewModel Get();

        Home GetForEdit();

        void Save(Home model);
    }
}