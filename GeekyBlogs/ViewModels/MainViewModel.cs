using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;
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

            if (e.NavigationMode != NavigationMode.Back)
            {
                if (e.Parameter is MenuItem)
                {
                    var menuItem = (MenuItem) e.Parameter;

                    if (menuItem.Title == "Geeky Theory")
                    {
                        Feeds = (await feedManagerService.GetFeedAsync(menuItem.Url)).ToObservableCollection();
                    }
                    else if (menuItem.Title == "Geeky Juegos")
                    {
                        Feeds = (await feedManagerService.GetFeedAsync(menuItem.Url)).ToObservableCollection();
                    }
                    else if(menuItem.View == typeof(MainView))
                    {
                        var tempList = new List<FeedItem>();
                        foreach (var item in MenuItems.Where(item => ApiHelper.ValidFeedUri(item.Url)))
                        {
                            tempList.AddRange(await feedManagerService.GetFeedAsync(item.Url));
                        }
                        tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));
                        tempList.ForEach(x =>
                        {
                            x.ColSpan = 1;
                            x.RowSpan = 1;
                        });
                        tempList[0].ColSpan = 2; tempList[0].RowSpan = 2;
                        tempList[1].ColSpan = 2; tempList[1].RowSpan = 2;
                        Feeds = tempList.ToObservableCollection();
                    }
                }
            }
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
