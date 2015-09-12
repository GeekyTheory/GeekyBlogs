using GeekyBlogs.ViewModels;
using GeekyTool;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GeekyBlogs.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellView : PageBase
    {
        public ShellView()
        {
            this.InitializeComponent();

            base.SplitViewFrame = SplitViewFrame;

            this.SizeChanged += ((ShellViewModel)this.DataContext).AppView_SizeChanged;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
        }
        
    }
}
