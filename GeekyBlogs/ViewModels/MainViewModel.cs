using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Common;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyBlogs.ViewModels.Base;
using GeekyTheory.Commands;
using GeekyTheory.ViewModels;

namespace GeekyBlogs.ViewModels
{
    public class MainViewModel : ViewModelBaseExtension
    {
        private readonly ILoadSplitterMenuService loadSplitterMenuService;
        private readonly IFeedManagerService feedManagerService;
        
        private List<FeedItem> feeds;
        private FeedItem feed;

        public MainViewModel(IFeedManagerService feedManagerService, ILoadSplitterMenuService loadSplitterMenuService)
        {
            this.feedManagerService = feedManagerService;
            this.loadSplitterMenuService = loadSplitterMenuService;

            Feeds = new List<FeedItem>();
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public override async Task OnNavigatedTo(NavigationEventArgs e)
        {
            MenuItems = loadSplitterMenuService.LoadMenu();

            //foreach (var item in MenuItems.Where(item => ApiHelper.ValidFeedUri(item.Url)))
            //{
            //    Feeds.AddRange(await feedManagerService.GetFeedAsync(item.Url));
            //}

            Feeds.AddRange(await feedManagerService.GetFeedAsync(MenuItems[2].Url));
            //Feeds.AddRange(await feedManagerService.GetFeedAsync(MenuItems[3].Url));
        }

        public List<FeedItem> Feeds
        {
            get { return feeds; }
            set
            {
                feeds = value;
                OnPropertyChanged();
            }
        }

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

    }
}
