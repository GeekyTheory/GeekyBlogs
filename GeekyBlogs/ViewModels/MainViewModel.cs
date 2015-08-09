using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyBlogs.Services;
using GeekyTheory.Commands;
using GeekyTheory.ViewModels;

namespace GeekyBlogs.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILoadSplitterMenuService loadSplitterMenuService;
        private readonly IFeedManagerService feedManagerService;

        private bool isPaneOpen;
        private ObservableCollection<MenuItem> menuItems;
        private MenuItem menuItem;
        private FeedData feeds;

        public MainViewModel(ILoadSplitterMenuService loadSplitterMenuService, IFeedManagerService feedManagerService)
        {
            this.loadSplitterMenuService = loadSplitterMenuService;
            this.feedManagerService = feedManagerService;

            HamburgerCommand = new DelegateCommand(HamburgerCommandDelegate);
        }

        public override Task OnNavigatedFrom(NavigationEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override async Task OnNavigatedTo(NavigationEventArgs e)
        {
            MenuItems = loadSplitterMenuService.LoadMenu();
            Feeds = await feedManagerService.GetFeedAsync(MenuItems[2].Url);
        }

        public bool IsPaneOpen
        {
            get { return isPaneOpen; }
            set
            {
                if (isPaneOpen != value)
                {
                    isPaneOpen = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return menuItems; }
            set
            {
                if (menuItems != value)
                {
                    menuItems = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public MenuItem MenuItem
        {
            get { return menuItem; }
            set
            {
                if (menuItem != value)
                {
                    menuItem = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public FeedData Feeds
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

        private double variableSizedGrid_Width;
        public double VariableSizedGrid_Width
        {
            get { return variableSizedGrid_Width; }
            set
            {
                if (variableSizedGrid_Width != value)
                {
                    variableSizedGrid_Width = value;
                    OnPropertyChanged();
                }
            }
        }


        private double viewWidth;
        public double ViewWidth
        {
            get { return viewWidth; }
            set
            {
                if (viewWidth != value)
                {
                    viewWidth = value;
                    OnPropertyChanged();
                }
            }
        }


        public ICommand HamburgerCommand { get; private set; }

        private void HamburgerCommandDelegate()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }
}
