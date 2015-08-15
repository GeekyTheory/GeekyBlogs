using Windows.UI.Xaml.Navigation;
using GeekyBlogs.ViewModels;
using GeekyTool;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GeekyBlogs.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : PageBase
    {
        public MainView()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.SizeChanged += ((MainViewModel)this.DataContext).AppView_SizeChanged;
        }
            
    }
}
