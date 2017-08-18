using System;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class PostsService : IPostsService
    {
        private readonly IMapper _mapper;
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly int _postsPerPage;

        public PostsService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService, IAppSettingsService appSettingsService,
            IMapper mapper)
        {
            _cultureService = cultureService;
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;

            _postsPerPage = appSettingsService.NumberOfItemsPerPage;
        }

        private string GetControllerName(PostType postType)
        {
            return _cultureService.IsCurrentCultureWelsh()
                ? (postType == PostType.Visit ? "Teithiau" : "Newyddion")
                : (postType == PostType.Visit ? "Visits" : "News");
        }

        public PostsViewModel Get(int page, PostType postType)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var totalItems = uow.Repository<Post>()
                    .Including(n => n.PostImages)
                    .Count(n => n.Type == postType);

                var maximumPageNumber = (double)totalItems / _postsPerPage;

                return new PostsViewModel
                {
                    PageNo = page,
                    Items = uow.Repository<Post>()
                        .Including(n => n.PostImages)
                        .Where(n => n.Type == postType)
                        .OrderByDescending(n => n.Published)
                        .Skip((page - 1) * _postsPerPage)
                        .Take(_postsPerPage)
                        .ToList()
                        .Select(n => _mapper.Map<Post,PostViewModel>(n))
                        .ToList(),
                    PreviousPage = page == 1 ? (int?)null : page - 1,
                    NextPage = (page >= maximumPageNumber) ? (int?)null : (page + 1),
                    ControllerName = GetControllerName(postType)
                };
            }
        }

        public PostViewModel Get(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var entity = uow.Repository<Post>()
                    .Including(n => n.PostImages)
                    .Single(p => p.Id == id);

                return _mapper.Map<Post, PostViewModel>(entity);
            }
        }

        public void Save(Post model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                if (model.Id > 0)
                {
                    uow.Repository<Post>().Update(model);
                }
                else
                {
                    model.Published = DateTime.Now;
                    uow.Repository<Post>().Insert(model);
                }

                uow.Commit();
            }
        }

        public Post GetForEdit(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Post>().GetById(id);
            }
        }

        public void Delete(Post model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Post>().Delete(model);

                uow.Commit();
            }
        }
    }
}