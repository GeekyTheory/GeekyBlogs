using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyBlogs.Views;
using GeekyTool.Models;
using GeekyTool.ViewModels;

namespace GeekyBlogs.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        private readonly IFeedManagerService feedManagerService;
        public ItemDetailViewModel(IFeedManagerService feedManagerService)
        {
            this.feedManagerService = feedManagerService;
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            return null;
        }

        public override async Task OnNavigatedTo(NavigationEventArgs e)
        {
            SetVisibilityOfNavigationBack();

            if (e.Parameter is FeedItem)
            {
                IsBusy = true;
                feed = (FeedItem) e.Parameter;
                if (string.IsNullOrEmpty(feed.Content))
                    feed.Content = await feedManagerService.RemoveUnusedElementsAsync(Feed.Link.ToString());
                OnPropertyChanged(nameof(Feed));
                IsBusy = false;
            }
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
    }
}
