using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.Domain.Concrete;
using System.Configuration;

namespace SportStore.WebUI.Infrastructure
{
    public class NinjetDependencyResolver:IDependencyResolver/// la clase
    {
        private IKernel Kernel;

        public NinjetDependencyResolver(IKernel kernelParam)
        {
            Kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type servciceType)
        {
            return Kernel.TryGet(servciceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product{Name="FootBall",Price=25},
            //    new Product{Name="SurfBoard",Price=179},
            //    new Product{Name="Running Shoes",Price=95}
            //});

            //Kernel.Bind<IProductRepository>().ToConstant(mock.Object) ;
            Kernel.Bind<IProductRepository>().To<EFProductRepository>();

            Emailsettings emailSettings = new Emailsettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            Kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                                            .WithConstructorArgument("settings", emailSettings);
        }
    }
}