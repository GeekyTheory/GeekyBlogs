using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.Views;
using GeekyTool.Models;
using GeekyTool.ViewModels;

namespace GeekyBlogs.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
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
    }
}
