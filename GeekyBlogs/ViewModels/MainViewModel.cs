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
using GeekyTool.Services.SplitterMenuService;
using GeekyTool.ViewModels;

namespace GeekyBlogs.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFeedManagerService feedManagerService;
        private readonly INavigationService navigationService;
        private readonly ISplitterMenuService splitterMenuService;
        
        private ObservableCollection<FeedItem> feeds;
        private FeedItem feed;

        public MainViewModel(IFeedManagerService feedManagerService, INavigationService navigationService, ISplitterMenuService splitterMenuService)
        {
            this.feedManagerService = feedManagerService;
            this.navigationService = navigationService;
            this.splitterMenuService = splitterMenuService;

            Feeds = new ObservableCollection<FeedItem>();
            AllFeeds = new List<FeedItem>();
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
                    IsBusy = true;
                    switch (menuItem.Title)
                    {
                        case CommonSettings.GEEKY_THEORY:
                            if (AllFeeds.Count > 1 && AllFeeds.Any(x => x.BlogItem.Title == CommonSettings.GEEKY_THEORY))
                                tempList.AddRange(AllFeeds.Where(item => item.BlogItem.Title.Contains(CommonSettings.GEEKY_THEORY)));
                            else
                                tempList = (await feedManagerService.GetFeedFromMenuItemAsync(menuItem));
                            Feeds = tempList.ToObservableCollection();
                            break;
                        case CommonSettings.GEEKY_JUEGOS:
                            if (AllFeeds.Count > 1 && AllFeeds.Any(x => x.BlogItem.Title == CommonSettings.GEEKY_JUEGOS))
                                tempList.AddRange(AllFeeds.Where(item => item.BlogItem.Title.Contains(CommonSettings.GEEKY_JUEGOS)));
                            else
                                tempList = (await feedManagerService.GetFeedFromMenuItemAsync(menuItem));
                            Feeds = tempList.ToObservableCollection();
                            break;
                        default:
                            if(menuItem.View == typeof(MainView))
                            {
                                foreach (var item in splitterMenuService.GetItems().Where(item => GeekyHelper.ValidFeedUri(item.Url)))
                                {
                                    tempList.AddRange(await feedManagerService.GetFeedFromMenuItemAsync(item));
                                    AllFeeds.AddRange(tempList);
                                }
                            }
                            break;
                    }
                    tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));
                    PrepareGridViewForSize(tempList);
                    IsBusy = false;
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


        private string currentSizeState;
        public string CurrentSizeState
        {
            get { return currentSizeState; }
            set
            {
                if (currentSizeState == value) return;
                currentSizeState = value;
                OnPropertyChanged();
            }
        }

        private void PrepareGridViewForSize(List<FeedItem> tempList)
        {
            if (tempList == null || tempList.Count == 0)
                return;

            if (CurrentSizeState != Enums.Size.OnehandState.ToString() && ViewWidth < (int)Enums.Size.OnehandState)
            {
                CurrentSizeState = Enums.Size.OnehandState.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 2;
                });
                tempList[0].RowSpan = 1; tempList[0].ColSpan = 4;

                Feeds = tempList.ToObservableCollection();
            }
            else if (CurrentSizeState != Enums.Size.MiddleState.ToString() 
                && ViewWidth < (int)Enums.Size.MiddleState && ViewWidth > (int)Enums.Size.OnehandState)
            {
                CurrentSizeState = Enums.Size.MiddleState.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 2;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2;

                Feeds = tempList.ToObservableCollection();
            }
            else if (CurrentSizeState != Enums.Size.DesktopState.ToString() 
                && ViewWidth > (int)Enums.Size.MiddleState)
            {
                CurrentSizeState = Enums.Size.DesktopState.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 1;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2;

                Feeds = tempList.ToObservableCollection();
            }
        }


        private List<FeedItem> allFeeds;
        public List<FeedItem> AllFeeds
        {
            get { return allFeeds; }
            set
            {
                if (allFeeds != value)
                {
                    allFeeds = value;
                    OnPropertyChanged();
                }
            }
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
                if (feed == value) return;
                feed = value;
                OnPropertyChanged();
                AppFrame.Navigate(typeof(ItemDetailView), Feed);
                feed = null;
            }
        }
    }
}
