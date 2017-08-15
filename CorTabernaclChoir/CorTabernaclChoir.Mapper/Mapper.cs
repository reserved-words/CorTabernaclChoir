using System;
using AutoMapper;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Mapper.Profiles;
using IMapper = CorTabernaclChoir.Common.Services.IMapper;

namespace CorTabernaclChoir.Mapper
{
    public class Mapper : IMapper
    {
        private readonly ICultureService _cultureService;
        private readonly Lazy<AutoMapper.IMapper> _englishAutoMapper;
        private readonly Lazy<AutoMapper.IMapper> _welshAutoMapper;

        public Mapper(ICultureService cultureService)
        {
            _cultureService = cultureService;
            _englishAutoMapper = new Lazy<AutoMapper.IMapper>(() => new AutoMapper.Mapper(ConfigureEnglishAutoMapper()));
            _welshAutoMapper = new Lazy<AutoMapper.IMapper>(() => new AutoMapper.Mapper(ConfigureWelshAutoMapper()));
        }

        private static MapperConfiguration ConfigureEnglishAutoMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile<EnglishProfile>();
            });
        }

        private static MapperConfiguration ConfigureWelshAutoMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile<WelshProfile>();
            });
        }

        public T1 Map<T2, T1>(T2 source, T1 destination)
        {
           return _cultureService.IsCurrentCultureWelsh()
                ? _welshAutoMapper.Value.Map(source, destination)
                : _englishAutoMapper.Value.Map(source, destination);
        }

        public T1 Map<T2, T1>(T2 source)
        {
            return _cultureService.IsCurrentCultureWelsh()
                ? _welshAutoMapper.Value.Map<T2, T1>(source)
                : _englishAutoMapper.Value.Map<T2, T1>(source);
        }
    }
}
