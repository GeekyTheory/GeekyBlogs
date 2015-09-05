using Autofac;
using GeekyBlogs.Services;
using GeekyTool.Services;
using GeekyTool.Services.SplitterMenuService;

namespace GeekyBlogs.ViewModels.Base
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
            builder.RegisterType<SplitterMenuService>().As<ISplitterMenuService>();

            // ViewModels
            builder.RegisterType<ShellViewModel>();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<ItemDetailViewModel>();

            container = builder.Build();
        }

        public ShellViewModel ShellViewModel { get { return container.Resolve<ShellViewModel>(); } }
        public MainViewModel MainViewModel { get { return container.Resolve<MainViewModel>(); } }
        public ItemDetailViewModel ItemDetailViewModel { get { return container.Resolve<ItemDetailViewModel>(); } }
    }
}
