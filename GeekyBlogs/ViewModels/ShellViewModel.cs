using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyBlogs.ViewModels.Base;
using GeekyBlogs.Views;
using GeekyTheory.Commands;
using GeekyTheory.ViewModels;

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
            PerformNavigationCommand = new DelegateCommand<MenuItem>(PerformNavigationCommandDelegate, null);
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs e)
        {
            MenuItems = loadSplitterMenuService.LoadMenu();

            return Task.FromResult(true);
        }

        public ICommand OpenPaneCommand { get; private set; }

        private void OpenPaneCommandDelegate()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public ICommand PerformNavigationCommand { get; private set; }

        private void PerformNavigationCommandDelegate(MenuItem item)
        {
            if (item.View == null)
                return;

            if (item.View == typeof(MainView))
            {
                while (SplitViewFrame.CanGoBack)
                {
                    SplitViewFrame.GoBack();
                }
            }
            SplitViewFrame.Navigate(item.View);

        }
    }
}
