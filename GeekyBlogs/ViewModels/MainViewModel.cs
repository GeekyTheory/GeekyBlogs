using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyBlogs.Views;
using GeekyTool;
using GeekyTool.Extensions;
using GeekyTool.Models;
using GeekyTool.Services;
using GeekyTool.ViewModels;

namespace GeekyBlogs.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFeedManagerService feedManagerService;
        private readonly INavigationService navigationService;
        
        private ObservableCollection<FeedItem> feeds;
        private FeedItem feed;

        public MainViewModel(IFeedManagerService feedManagerService, INavigationService navigationService)
        {
            this.feedManagerService = feedManagerService;
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

            if (e.NavigationMode != NavigationMode.Back)
            {
                if (e.Parameter is MenuItem)
                {
                    var menuItem = (MenuItem) e.Parameter;

                    var tempList = new List<FeedItem>();
                    if (menuItem.Title == "Geeky Theory")
                    {
                        tempList = (await feedManagerService.GetFeedAsync(menuItem.Url));
                    }
                    else if (menuItem.Title == "Geeky Juegos")
                    {
                        tempList = (await feedManagerService.GetFeedAsync(menuItem.Url));
                    }
                    else if(menuItem.View == typeof(MainView))
                    {
                        foreach (var item in MenuItems.instance.Items.Where(item => GeekyHelper.ValidFeedUri(item.Url)))
                        {
                            tempList.AddRange(await feedManagerService.GetFeedAsync(item.Url));
                        }
                    }
                    PrepareGridViewForSize(tempList);
                }
            }
        }

        private double previousSize = 0;

        public override void AppView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.AppView_SizeChanged(sender, e);

            PrepareGridViewForSize(Feeds.ToList());
            previousSize = ViewWidth;
        }

        private void PrepareGridViewForSize(List<FeedItem> tempList)
        {
            if (tempList == null || tempList.Count == 0)
                return;

            if (ViewWidth < (int)Enums.Size.OnehandState)
            {
                tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));
                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 2;
                });
                tempList[0].RowSpan = 1; tempList[0].ColSpan = 4;
            }
            else if (ViewWidth < (int)Enums.Size.MiddleState)
            {
                tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));
                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 2;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2;
            }
            else if (ViewWidth < (int)Enums.Size.DesktopState)
            {
                tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));
                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 1;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2; 
            }

            Feeds = tempList.ToObservableCollection();
        }


        public ObservableCollection<FeedItem> Feeds
        {
            get { return feeds; }
            set
            {
                if (feeds != value)
                {
                    feeds = value;
                    OnPropertyChanged();
                }
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
