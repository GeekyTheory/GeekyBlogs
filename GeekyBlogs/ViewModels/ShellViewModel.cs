using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Views;
using GeekyTool.Commands;
using GeekyTool.Models;
using GeekyTool.ViewModels;
using System;

namespace GeekyBlogs.ViewModels
{
    public class ShellViewModel : SplitterViewModelBase
    {
        public ShellViewModel()
        {
            SetVisibilityOfNavigationBack();
            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            OpenPaneCommand = new DelegateCommand(OpenPaneCommandDelegate);
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public async override Task OnNavigatedTo(NavigationEventArgs e)
        {
            var items = new List<MenuItem>()
            {
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Icons/Dashboard.png",
                    Title = "Portada",
                    Brush = "#212121",
                    View = typeof (MainView)
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Geeky/geeky_theory_icon_round.png",
                    Title = CommonSettings.GEEKY_THEORY,
                    Brush = "#1ABB9C",
                    Url = "https://geekytheory.com/feed",
                    View = typeof (MainView)
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Geeky/geeky_juegos_icon_round.png",
                    Title = CommonSettings.GEEKY_JUEGOS,
                    Brush = "#FF6C60",
                    Url = "https://geekyjuegos.com/feed",
                    View = typeof (MainView)
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Icons/Category.png",
                    Title = "Categorías",
                }
            };

            SplitterMenuService.AddItems(items);
            
            MenuItem = MenuItems.FirstOrDefault(x => x.View == typeof (MainView));

            await HideStatusBarOnMobileAsync();

            return;
        }

        public ICommand OpenPaneCommand { get; private set; }

        private void OpenPaneCommandDelegate()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        protected override void PerformNavigationCommandDelegate(MenuItem item)
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
            SplitViewFrame.Navigate(item.View, item);
        }

        private async Task HideStatusBarOnMobileAsync()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
        }
    }
}
