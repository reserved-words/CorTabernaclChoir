namespace CorTabernaclChoir.Common.Services
{
    public interface IMapper
    {
        T1 Map<T2, T1>(T2 source, T1 destination);
        T1 Map<T2, T1>(T2 source);
    }
}
