using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.ViewModels;
using GeekyTool;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GeekyBlogs.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDetailView : PageBase
    {
        public ItemDetailView()
        {
            this.InitializeComponent();

            this.SizeChanged += ((ItemDetailViewModel)this.DataContext).AppView_SizeChanged;
        }
    }
}
