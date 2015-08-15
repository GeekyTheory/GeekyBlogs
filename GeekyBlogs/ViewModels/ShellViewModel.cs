using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyBlogs.ViewModels.Base;
using GeekyBlogs.Views;
using GeekyTool.Commands;

namespace GeekyBlogs.ViewModels
{
    public class ShellViewModel : ViewModelBaseExtension
    {
        private readonly ILoadSplitterMenuService loadSplitterMenuService;

        public ShellViewModel(ILoadSplitterMenuService loadSplitterMenuService)
        {
            this.loadSplitterMenuService = loadSplitterMenuService;

            SetVisibilityOfNavigationBack();
            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            OpenPaneCommand = new DelegateCommand(OpenPaneCommandDelegate);
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs e)
        {
            MenuItems = loadSplitterMenuService.LoadMenu();

            MenuItem = MenuItems.FirstOrDefault(x => x.View == typeof (MainView));

            return Task.FromResult(true);
        }

        public ICommand OpenPaneCommand { get; private set; }

        private void OpenPaneCommandDelegate()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }
}
