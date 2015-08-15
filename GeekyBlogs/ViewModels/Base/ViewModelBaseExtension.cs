using System.Collections.ObjectModel;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GeekyBlogs.Models;
using GeekyBlogs.Views;
using GeekyTool.ViewModels;

namespace GeekyBlogs.ViewModels.Base
{
    public abstract class ViewModelBaseExtension : ViewModelBase
    {
        private bool isPaneOpen;
        private ObservableCollection<MenuItem> menuItems;
        private MenuItem menuItem;

        public bool IsPaneOpen
        {
            get { return isPaneOpen; }
            set
            {
                if (isPaneOpen != value)
                {
                    isPaneOpen = value;
                    OnPropertyChanged();
                    GetCalculatedVariableSize(ViewWidth);
                }
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return menuItems; }
            set
            {
                if (menuItems != value)
                {
                    menuItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public MenuItem MenuItem
        {
            get { return menuItem; }
            set
            {
                if (menuItem != value)
                {
                    menuItem = value;
                    OnPropertyChanged();
                    PerformNavigationCommandDelegate(menuItem);
                    menuItem = null;
                }
            }
        }

        public void GetCalculatedVariableSize(double width)
        {
            VariableSizedGrid_Width = width / 4;
        }

        public void AppView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewWidth = e.NewSize.Width;

            GetCalculatedVariableSize(ViewWidth);

        }

        public void SetVisibilityOfNavigationBack()
        {
            var currentView = SystemNavigationManager.GetForCurrentView();

            if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                if (AppFrame != null && AppFrame.CanGoBack)
                {
                    currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
                else if (SplitViewFrame != null && SplitViewFrame.CanGoBack)
                {
                    currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        public void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (AppFrame != null && AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
                e.Handled = true;
            }
            else if (SplitViewFrame != null && SplitViewFrame.CanGoBack)
            {
                SplitViewFrame.GoBack();
                e.Handled = true;
            }
        }

        public void PerformNavigationCommandDelegate(MenuItem item)
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
    }
}
