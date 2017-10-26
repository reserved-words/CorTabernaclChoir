using System;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Common.Delegates;

namespace CorTabernaclChoir.Services
{
    public class EventsService : IEventsService
    {
        private readonly IMapper _mapper;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly GetCurrentTime _getCurrentTime;

        public EventsService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper, GetCurrentTime getCurrentTime)
        {
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
            _getCurrentTime = getCurrentTime;
        }

        public EventsViewModel GetAll()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var currentTime = _getCurrentTime();

                return new EventsViewModel
                {
                    Upcoming = uow.Repository<Event>()
                        .Including(n => n.PostImages)
                        .Where(n => n.Date >= currentTime)
                        .OrderBy(n => n.Date)
                        .ToList()
                        .Select(n => _mapper.Map<Event,EventViewModel>(n))
                        .ToList(),
                    Past = uow.Repository<Event>()
                        .Including(n => n.PostImages)
                        .Where(n => n.Date < currentTime)
                        .OrderByDescending(n => n.Date)
                        .ToList()
                        .Select(n => _mapper.Map<Event, EventSummaryViewModel>(n))
                        .ToList()
                };
            }
        }

        public EventViewModel GetById(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var entity = uow.Repository<Event>()
                    .Including(e => e.PostImages)
                    .Single(e => e.Id == id);

                return _mapper.Map<Event, EventViewModel>(entity);
            }
        }

        public int Save(EditEventViewModel model)
        {
            var post = _mapper.Map<EditEventViewModel, Event>(model);

            using (var uow = _unitOfWorkFactory())
            {
                if (model.Id > 0)
                {
                    uow.Repository<Event>().Update(post);

                    foreach (var image in model.PostImages.Where(im => im.MarkForDeletion))
                    {
                        uow.Repository<PostImage>().Delete(image.Id);
                    }
                }
                else
                {
                    post.Published = _getCurrentTime();
                    uow.Repository<Event>().Insert(post);
                }

                uow.Commit();

                return model.Id;
            }
        }

        public EditEventViewModel GetForEdit(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var entity = uow.Repository<Event>()
                    .Including(e => e.PostImages)
                    .Single(e => e.Id == id);

                return _mapper.Map<Event, EditEventViewModel>(entity);
            }
        }

        public void Delete(Event model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Event>().Delete(model);

                uow.Commit();
            }
        }

        public int SaveImage(int postId, string fileExtension)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var postImage = new PostImage
                {
                    PostId = postId,
                    FileExtension = fileExtension
                };

                uow.Repository<PostImage>()
                    .Insert(postImage);

                uow.Commit();

                return postImage.Id;
            }
        }
        
        public void DeleteImage(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var image = uow.Repository<PostImage>().GetById(id);

                image.Post.PostImages.Remove(image);

                uow.Repository<PostImage>().Delete(id);

                uow.Commit();
            }
        }
    }
}