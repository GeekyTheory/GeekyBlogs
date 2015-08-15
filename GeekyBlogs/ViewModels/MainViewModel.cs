using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Common;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyBlogs.ViewModels.Base;
using GeekyBlogs.Views;
using GeekyTool.Extensions;
using GeekyTool.Services;

namespace GeekyBlogs.ViewModels
{
    public class MainViewModel : ViewModelBaseExtension
    {
        private readonly ILoadSplitterMenuService loadSplitterMenuService;
        private readonly IFeedManagerService feedManagerService;
        private readonly INavigationService navigationService;
        
        private ObservableCollection<FeedItem> feeds;
        private FeedItem feed;

        public MainViewModel(IFeedManagerService feedManagerService, ILoadSplitterMenuService loadSplitterMenuService, INavigationService navigationService)
        {
            this.feedManagerService = feedManagerService;
            this.loadSplitterMenuService = loadSplitterMenuService;
            this.navigationService = navigationService;

            Feeds = new ObservableCollection<FeedItem>();
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public override async Task OnNavigatedTo(NavigationEventArgs e)
        {
            SetVisibilityOfNavigationBack();

            MenuItems = loadSplitterMenuService.LoadMenu();

            var tempList = new List<FeedItem>();
            foreach (var item in MenuItems.Where(item => ApiHelper.ValidFeedUri(item.Url)))
            {
                tempList.AddRange(await feedManagerService.GetFeedAsync(item.Url));
            }
            Feeds = tempList.ToObservableCollection();
        }


        public ObservableCollection<FeedItem> Feeds
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
                    AppFrame.Navigate(typeof(ItemDetailView), Feed);
                    feed = null;
                }
            }
        }
    }
}
