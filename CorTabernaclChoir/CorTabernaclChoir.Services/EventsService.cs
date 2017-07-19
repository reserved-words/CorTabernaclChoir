using System;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class EventsService : IEventsService
    {
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly int _itemsPerPage;

        public EventsService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService, IAppSettingsService appSettingsService)
        {
            _cultureService = cultureService;
            _unitOfWorkFactory = unitOfWorkFactory;

            _itemsPerPage = appSettingsService.NumberOfItemsPerPage;
        }

        public EventsViewModel Get(int page)
        {
            var isCurrentCultureWelsh = _cultureService.IsCurrentCultureWelsh();

            using (var uow = _unitOfWorkFactory())
            {
                var totalItems = uow.Repository<Event>()
                    .Including(n => n.PostImages)
                    .Count();

                var maximumPageNumber = (double)totalItems / _itemsPerPage;

                return new EventsViewModel
                {
                    PageNo = page,
                    Items = uow.Repository<Event>()
                        .Including(n => n.PostImages)
                        .OrderByDescending(n => n.Date)
                        .Skip((page - 1) * _itemsPerPage)
                        .Take(_itemsPerPage)
                        .Select(n => new EventViewModel
                        {
                            Id = n.Id,
                            Title = isCurrentCultureWelsh ? n.Title_W : n.Title_E,
                            Venue = isCurrentCultureWelsh ? n.Venue_W : n.Venue_E,
                            Address = isCurrentCultureWelsh ? n.Address_W : n.Address_E,
                            Date = n.Date,
                            Images = n.PostImages.Select(im => im.Id).ToList()
                        })
                        .ToList(),
                    PreviousPage = page == 1 ? (int?)null : page - 1,
                    NextPage = (page >= maximumPageNumber) ? (int?)null : (page + 1)
                };
            }
        }

        public void Save(Event model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                if (model.Id > 0)
                {
                    uow.Repository<Event>().Update(model);
                }
                else
                {
                    uow.Repository<Event>().Insert(model);
                }

                uow.Commit();
            }
        }

        public Event GetForEdit(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Event>().GetById(id);
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
    }
}