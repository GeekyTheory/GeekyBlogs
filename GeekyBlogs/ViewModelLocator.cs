using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using GeekyBlogs.Services;
using GeekyBlogs.ViewModels;
using GeekyTheory.Services;

namespace GeekyBlogs
{
    public class ViewModelLocator
    {
        private IContainer container;

        public ViewModelLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //Interfaces
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<LocalSettingsService>().As<ILocalSettingsService>();
            builder.RegisterType<FeedManagerService>().As<IFeedManagerService>();
            builder.RegisterType<LoadSplitterMenuService>().As<ILoadSplitterMenuService>();

            // ViewModels
            builder.RegisterType<ShellViewModel>();
            builder.RegisterType<MainViewModel>();

            container = builder.Build();
        }

        public ShellViewModel ShellViewModel { get { return container.Resolve<ShellViewModel>(); } }
        public MainViewModel MainViewModel { get { return container.Resolve<MainViewModel>(); } }
    }
}
