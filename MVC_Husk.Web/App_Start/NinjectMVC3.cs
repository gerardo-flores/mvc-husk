[assembly: WebActivator.PreApplicationStartMethod(typeof(MVC_Husk.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MVC_Husk.App_Start.NinjectMVC3), "Stop")]

namespace MVC_Husk.App_Start
{
    using System.Web.Mvc;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using MVC_Husk.Infrastructure.Attributes;
    using MVC_Husk.Infrastructure.IdStore;
    using MVC_Husk.Infrastructure.Logging;
    using Ninject;
    using Ninject.Web.Mvc;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using MVC_Husk.Infrastructure.BackgroundJobs;
    using Quartz;
    using Quartz.Impl;

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
            kernel.Bind<ILogger>().To<NLogger>().InSingletonScope();
            kernel.Bind<IIdStore>().To<FormsAuthIdStore>();
            kernel.Bind<IBackgroundJobManager>().To<BackgroundJobManager>().InSingletonScope();
            kernel.Bind<ISchedulerFactory>().To<StdSchedulerFactory>().InSingletonScope();
            kernel.Bind<IScheduler>().ToMethod(context => context.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();
            kernel.BindFilter<VerifyClickThruAttribute>(FilterScope.Controller, 0);
        }        
    }
}
