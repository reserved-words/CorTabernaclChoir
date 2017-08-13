using System;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class LoggingService : ILogger
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public LoggingService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Info(string message)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Log>().Insert(new Log
                {
                    LoggedAt = DateTime.Now,
                    Message = message
                });
            }
        }

        public void Error(Exception ex, string requestUrl, string message)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Log>().Insert(GetExceptionLog(ex, requestUrl, message));

                while (ex.InnerException != null)
                {
                    var innerExceptionLog = GetExceptionLog(ex.InnerException, requestUrl, "Inner Exception");
                    uow.Repository<Log>().Insert(innerExceptionLog);
                    ex = ex.InnerException;
                }

                uow.Commit();
            }
        }

        private static Log GetExceptionLog(Exception ex, string requestUrl = null, string message = null)
        {
            return new Log
            {
                LoggedAt = DateTime.Now,
                Message = message,
                ExceptionType = ex.GetType().Name,
                ExceptionMessage = ex.Message,
                RequestUrl = requestUrl,
                StackTrace = ex.StackTrace
            };
        }
    }
}
