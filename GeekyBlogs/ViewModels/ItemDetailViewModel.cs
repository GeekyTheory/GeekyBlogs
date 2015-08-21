using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.ViewModels.Base;
using GeekyBlogs.Views;
using MenuItem = GeekyTool.Models.MenuItem;

namespace GeekyBlogs.ViewModels
{
    public class ItemDetailViewModel : ViewModelBaseExtension
    {
        public ItemDetailViewModel()
        {
            
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs e)
        {
            SetVisibilityOfNavigationBack();

            if (e.Parameter is FeedItem)
            {
                Feed = (FeedItem) e.Parameter;
            }

            return Task.FromResult(true);
        }



        private FeedItem feed;
        public FeedItem Feed
        {
            get { return feed; }
            set
            {
                if (feed != value)
                {
                    feed = value;
                    OnPropertyChanged();
                }
            }
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
    }
}
