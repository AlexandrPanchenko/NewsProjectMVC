using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Concrete;
using NewsDotNet.WebUI.Areas.Admin.Mappers;

namespace NewsDotNet.WebUI.Infrastracture
{
    /// <summary>
    /// Responsible for resolving dependencies when ASP.NET attempts to create objects
    /// </summary>
    public class NinjectDependencyResolver:IDependencyResolver
    {
        private IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            RegisterServices();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void RegisterServices()
        {
            //Put all interfaces to implementations bindigs here
            //Example: RegisterService<ITagsRepository, TagsRepository>();
            this.RegisterService<IArticlesRepository, ArticleRepository>()
                .RegisterService<ITagsRepository, TagsRepository>()
                .RegisterService<IMainPageEntitiesRepository, MainPageEntitiesRepository>()
                .RegisterService<IMapper, CommonMapper>();
        }


        private NinjectDependencyResolver RegisterService<TInterface, TImplementation>()
            where TImplementation: class, TInterface
        {
            _kernel.Bind<TInterface>().To<TImplementation>();
            return this;
        }
    }
}