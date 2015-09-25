using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
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

        private void VariableGridView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //OutstandingVariableGridView.SelectedIndex = -1;
            //VariableGridView.SelectedIndex = -1;
        }

        private void HeroFlipItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var ctx = (MainViewModel) this.DataContext;
            ctx.Feed = (FeedItem) (sender as FlipView).SelectedItem;
        }
    }
}
