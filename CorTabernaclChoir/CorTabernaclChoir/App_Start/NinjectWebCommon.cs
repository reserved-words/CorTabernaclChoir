using CorTabernaclChoir.Common.Delegates;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Interfaces;
using CorTabernaclChoir.Messages;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CorTabernaclChoir.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CorTabernaclChoir.App_Start.NinjectWebCommon), "Stop")]

namespace CorTabernaclChoir.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using Ninject.Extensions.Conventions;
    using Data;
    using Data.Contracts;
    using Services;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<Func<IUnitOfWork>>().ToMethod(context =>
            {
                return (() => new UnitOfWork());
            });

            kernel.Bind<GetCurrentTime>().ToMethod(context =>
            {
                return (() => DateTime.Now);
            });

            kernel.Bind(x => x.FromAssemblyContaining<HomeService>()
                .SelectAllClasses()
                .InNamespaceOf<HomeService>()
                .BindDefaultInterface());

            kernel.Bind<IEmailService>().To<ContactEmailService>();

            kernel.Bind<IUploadedFileValidator>().To<UploadedFileValidator>();
            kernel.Bind<IUploadedFileService>().To<UploadedFileService>();

            kernel.Bind<ILogger>().To<LoggingService>();
            kernel.Bind<IMessageContainer>().To<MessageContainer>();

            kernel.Bind<IMapper>().To<Mapper.Mapper>().InSingletonScope();
            kernel.Bind<IAppSettingsService>().To<AppSettingsService>().InSingletonScope();
        }
    }
}
