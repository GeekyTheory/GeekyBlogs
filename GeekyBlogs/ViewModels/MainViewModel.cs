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
        private readonly ISplitterMenuService splitterMenuService;

        private ObservableCollection<FeedItem> feeds;
        private List<FeedItem> allFeeds;
        private FeedItem feed;
        private string currentSizeState;

        public MainViewModel(IFeedManagerService feedManagerService, ISplitterMenuService splitterMenuService)
        {
            this.feedManagerService = feedManagerService;
            this.splitterMenuService = splitterMenuService;

            Feeds = new ObservableCollection<FeedItem>();
            AllFeeds = new List<FeedItem>();
            VariableSizedGrid_Height = 240;
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
                                    AllFeeds = tempList;
                                }
                            }
                            Feeds = tempList.ToObservableCollection();
                            break;
                    }
                    tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));
                    PrepareGridViewForSize(tempList);
                    IsBusy = false;
                }
            }
        }

        public override void AppView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.AppView_SizeChanged(sender, e);

            PrepareGridViewForSize(Feeds.ToList());
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
                if (Feed != null)
                    AppFrame.Navigate(typeof (ItemDetailView), Feed);
            }
        }

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


        private double variableSizedGrid_Height;
        public double VariableSizedGrid_Height
        {
            get { return variableSizedGrid_Height; }
            set
            {
                if (variableSizedGrid_Height == value) return;
                variableSizedGrid_Height = value;
                OnPropertyChanged();
            }
        }


        private void PrepareGridViewForSize(List<FeedItem> tempList)
        {
            if (tempList == null || tempList.Count == 0)
                return;

            // SmallDevices 0px - 768px
            if (CurrentSizeState != Enums.Size.SmallDevices.ToString()
                && ViewWidth >= (int)Enums.Size.SmallDevices
                && ViewWidth < (int)Enums.Size.MediumDevices)
            {
                CurrentSizeState = Enums.Size.SmallDevices.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 2;
                });
                tempList[0].RowSpan = 1; tempList[0].ColSpan = 4;

                Feeds = tempList.ToObservableCollection();
            }
            // MediumDevices 768px - 992px
            else if (CurrentSizeState != Enums.Size.MediumDevices.ToString()
                && ViewWidth >= (int)Enums.Size.MediumDevices
                && ViewWidth < (int)Enums.Size.LargeDevices)
            {
                CurrentSizeState = Enums.Size.MediumDevices.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 2;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2;

                Feeds = tempList.ToObservableCollection();
            }
            // LargeDevices 992px - 1200px
            else if (CurrentSizeState != Enums.Size.LargeDevices.ToString()
                && ViewWidth >= (int)Enums.Size.LargeDevices
                && ViewWidth < (int)Enums.Size.XLargeDevices)
            {
                CurrentSizeState = Enums.Size.LargeDevices.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 1;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2;

                Feeds = tempList.ToObservableCollection();
            }
            // XLargeDevices > 1200px
            else if (CurrentSizeState != Enums.Size.XLargeDevices.ToString()
                && ViewWidth >= (int)Enums.Size.XLargeDevices)
            {
                CurrentSizeState = Enums.Size.XLargeDevices.ToString();

                tempList.ForEach(x =>
                {
                    x.RowSpan = 1; x.ColSpan = 1;
                });
                tempList[0].RowSpan = 2; tempList[0].ColSpan = 2;
                tempList[1].RowSpan = 2; tempList[1].ColSpan = 2;

                Feeds = tempList.ToObservableCollection();
            }
        }
    }
}
