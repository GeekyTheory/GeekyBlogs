using System;
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

        private List<FeedItem> allFeeds;
        private ObservableCollection<FeedItem> outstandingFeeds;
        private List<FeedItem> feeds;
        private FeedItem feed;
        private Enums.Size currentSizeState;

        private DispatcherTimer changeHeroFeedTimer;
        private int outstandingFeedsSelectedIndex;

        private double variableSizedGrid_Height;

        public MainViewModel(IFeedManagerService feedManagerService, ISplitterMenuService splitterMenuService)
        {
            this.feedManagerService = feedManagerService;
            this.splitterMenuService = splitterMenuService;

            AllFeeds = new List<FeedItem>();
            OutstandingFeeds = new ObservableCollection<FeedItem>();
            Feeds = new List<FeedItem>();

            changeHeroFeedTimer = new DispatcherTimer();

            VariableSizedGrid_Height = 300;
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            changeHeroFeedTimer.Stop();
            changeHeroFeedTimer.Tick -= ChangeActiveHeroFeed;
            return null;
        }

        public override async Task OnNavigatedTo(NavigationEventArgs e)
        {
            SetVisibilityOfNavigationBack();

            outstandingFeedsSelectedIndex = 0;
            changeHeroFeedTimer.Interval = TimeSpan.FromSeconds(5);
            changeHeroFeedTimer.Tick += ChangeActiveHeroFeed;
            changeHeroFeedTimer.Start();

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
                                tempList = AllFeeds.Where(item => item.BlogItem.Title.Contains(CommonSettings.GEEKY_THEORY)).ToList();
                            else
                                tempList = (await feedManagerService.GetFeedFromMenuItemAsync(menuItem));
                            break;
                        case CommonSettings.GEEKY_JUEGOS:
                            if (AllFeeds.Count > 1 && AllFeeds.Any(x => x.BlogItem.Title == CommonSettings.GEEKY_JUEGOS))
                                tempList = AllFeeds.Where(item => item.BlogItem.Title.Contains(CommonSettings.GEEKY_JUEGOS)).ToList();
                            else
                                tempList = (await feedManagerService.GetFeedFromMenuItemAsync(menuItem));
                            break;
                        default:
                            if(menuItem.View == typeof(MainView))
                            {
                                if (AllFeeds.Count == 0)
                                {
                                    foreach (
                                        var item in
                                            splitterMenuService.GetItems()
                                                .Where(item => GeekyHelper.ValidFeedUri(item.Url)))
                                    {
                                        tempList.AddRange(await feedManagerService.GetFeedFromMenuItemAsync(item));
                                        AllFeeds = tempList;
                                    }
                                }
                                else
                                    tempList = AllFeeds;
                            }
                            break;
                    }
                    tempList.Sort((a, b) => b.PubDate.CompareTo(a.PubDate));

                    var outstandings = tempList.Take(3).ToList();
                    OutstandingFeeds.Clear();
                    foreach (var feed in outstandings)
                    {
                        OutstandingFeeds.Add(feed);
                    }

                    feeds = tempList.Skip(3).TakeWhile(x => true).ToList();

                    // Fill items
                    //switch (CurrentSizeState)
                    //{
                    //    case Enums.Size.SmallDevices:
                    //        OutstandingFeeds = tempList.Take(2).ToList();
                    //        Feeds = tempList.Skip(2).TakeWhile(x => true).ToList();
                    //        break;
                    //    case Enums.Size.MediumDevices:
                    //        OutstandingFeeds = tempList.Take(2).ToList();
                    //        Feeds = tempList.Skip(2).TakeWhile(x => true).ToList();
                    //        break;
                    //    case Enums.Size.LargeDevices:
                    //        OutstandingFeeds = tempList.Take(2).ToList();
                    //        Feeds = tempList.Skip(2).TakeWhile(x => true).ToList();
                    //        break;
                    //    case Enums.Size.XLargeDevices:
                    //        OutstandingFeeds = tempList.Take(3).ToList();
                    //        Feeds = tempList.Skip(3).TakeWhile(x => true).ToList();
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}
                    CurrentSizeState = Enums.Size.None;
                    PrepareAllFeedItemsForDisplay();
                    IsBusy = false;
                }
            }
        }

        private void ChangeActiveHeroFeed(object sender, object e)
        {
            if (OutstandingFeedsSelectedIndex == 2)
                OutstandingFeedsSelectedIndex = 0;
            else
                OutstandingFeedsSelectedIndex += 1;
        }

        public override void AppView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.AppView_SizeChanged(sender, e);

            PrepareAllFeedItemsForDisplay();
        }

        public List<FeedItem> AllFeeds
        {
            get { return allFeeds; }
            set
            {
                if (allFeeds == value) return;
                allFeeds = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FeedItem> OutstandingFeeds
        {
            get { return outstandingFeeds; }
            set
            {
                if (outstandingFeeds == value) return;
                outstandingFeeds = value;
                OnPropertyChanged();
            }
        }

        public List<FeedItem> Feeds
        {
            get { return feeds; }
            set
            {
                if (feeds == value) return;
                feeds = value;
                OnPropertyChanged();
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

        public Enums.Size CurrentSizeState
        {
            get { return currentSizeState; }
            set
            {
                if (currentSizeState == value) return;
                currentSizeState = value;
                OnPropertyChanged();
            }
        }

        public int OutstandingFeedsSelectedIndex
        {
            get { return outstandingFeedsSelectedIndex; }
            set
            {
                if (outstandingFeedsSelectedIndex == value) return;
                outstandingFeedsSelectedIndex = value;
                OnPropertyChanged();
            }
        }

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

        private void PrepareAllFeedItemsForDisplay()
        {
            // SmallDevices 0px - 768px
            if (CurrentSizeState != Enums.Size.SmallDevices && ViewWidth >= (int) Enums.Size.SmallDevices && ViewWidth < (int) Enums.Size.MediumDevices)
            {
                CurrentSizeState = Enums.Size.SmallDevices;

                //if (outstandingFeeds.Count > 1)
                //{
                //    var tempList = outstandingFeeds.Skip(1).TakeWhile(x => true).ToList();
                //    foreach (var item in tempList)
                //    {
                //        outstandingFeeds.Remove(item);
                //    }
                //    var test = feeds;
                //    test.AddRange(tempList);
                //    feeds = test;
                //}

                // Set specific size,
                outstandingFeeds.ToList().ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 4;
                });
                feeds.ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 2;
                });

                // Update the view
                OnPropertyChanged(nameof(OutstandingFeeds));
                OnPropertyChanged(nameof(Feeds));
            }
            // MediumDevices 768px - 992px
            else if (CurrentSizeState != Enums.Size.MediumDevices && ViewWidth >= (int) Enums.Size.MediumDevices && ViewWidth < (int) Enums.Size.LargeDevices)
            {
                CurrentSizeState = Enums.Size.MediumDevices;

                // Set specific size
                outstandingFeeds.ToList().ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 2;
                });
                feeds.ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 1;
                });

                // Update the view
                OnPropertyChanged(nameof(OutstandingFeeds));
                OnPropertyChanged(nameof(Feeds));
            }
            // LargeDevices 992px - 1200px
            else if (CurrentSizeState != Enums.Size.LargeDevices && ViewWidth >= (int) Enums.Size.LargeDevices && ViewWidth < (int) Enums.Size.XLargeDevices)
            {
                CurrentSizeState = Enums.Size.LargeDevices;

                // Set specific size
                outstandingFeeds.ToList().ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 2;
                });
                feeds.ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 1;
                });

                // Update the view
                OnPropertyChanged(nameof(OutstandingFeeds));
                OnPropertyChanged(nameof(Feeds));
            }
            // XLargeDevices > 1200px
            else if (CurrentSizeState != Enums.Size.XLargeDevices && ViewWidth >= (int) Enums.Size.XLargeDevices)
            {
                CurrentSizeState = Enums.Size.XLargeDevices;

                // Set specific size
                outstandingFeeds.ToList().ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 2;
                });
                feeds.ForEach(x =>
                {
                    x.RowSpan = 1;
                    x.ColSpan = 1;
                });

                // Update the view
                OnPropertyChanged(nameof(OutstandingFeeds));
                OnPropertyChanged(nameof(Feeds));
            }
            else
            {
                // Update the view
                OnPropertyChanged(nameof(OutstandingFeeds));
                OnPropertyChanged(nameof(Feeds));
            }
        }
    }
}
