[assembly: WebActivator.PreApplicationStartMethod(typeof(MVC_Husk.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MVC_Husk.App_Start.NinjectMVC3), "Stop")]

namespace MVC_Husk.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;
    using MVC_Husk.Infrastructure.Logging;
    using MVC_Husk.Infrastructure.BackgroundJobs;
    using Quartz;
    using Quartz.Impl;
    using MVC_Husk.Infrastructure.IdStore;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
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
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ILogger>().To<NLogger>();
            kernel.Bind<IBackgroundJobManager>().To<BackgroundJobManager>();
            kernel.Bind<ISchedulerFactory>().To<StdSchedulerFactory>().InSingletonScope();
            kernel.Bind<IScheduler>().ToMethod(context => context.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();
            kernel.Bind<IIdStore>().To<FormsAuthIdStore>();
        }        
    }
}
