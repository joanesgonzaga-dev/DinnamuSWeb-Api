using DinnamuSWebApi.Repositories;
using DinnamuSWebApi.Repositories.Grades;
using DinnamuSWebApi.Repositories.Produtos;
using DinnamuSWebApi.Repositories.Vendas;
using Ninject;
using Ninject.Extensions.ChildKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace DinnamuSWebApi.Infraestructure
{
        public class NinjectResolver : IDependencyResolver
        {
            private IKernel kernel;

            public NinjectResolver()
                : this(new StandardKernel())
            {
            }

            public NinjectResolver(IKernel ninjectKernel, bool scope = false)
            {
                kernel = ninjectKernel;
                if (!scope)
                {
                    AddBindings(kernel);
                }
            }

            public IDependencyScope BeginScope()
            {
                return new NinjectResolver(AddRequestBindings(new ChildKernel(kernel)), true);
            }

            public object GetService(Type serviceType)
            {
                return kernel.TryGet(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return kernel.GetAll(serviceType);
            }

            public void Dispose()
            {

            }

            private void AddBindings(IKernel kernel)
            {
                // singleton and transient bindings go here
            }

            private IKernel AddRequestBindings(IKernel kernel)
            {
                kernel.Bind<IClienteRepository>().To<ClienteRepository>().InSingletonScope();
                kernel.Bind<IProdutoRepository>().To<ProdutoRepository>().InSingletonScope();
                kernel.Bind<IGradeRepository>().To<GradeRepository>().InSingletonScope();
                kernel.Bind<ICotacaoRepository>().To<CotacaoRepository>().InSingletonScope();
                return kernel;
            }
        }
}