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

        private const string DisableScrollingJs = @"function RemoveScrolling()
                              {
                                  var styleElement = document.createElement('style');
                                  var styleText = 'body, html { overflow: hidden; }'
                                  var headElements = document.getElementsByTagName('head');
                                  styleElement.type = 'text/css';
                                  if (headElements.length == 1)
                                  {
                                      headElements[0].appendChild(styleElement);
                                  }
                                  else if (document.head)
                                  {
                                      document.head.appendChild(styleElement);
                                  }
                                  if (styleElement.styleSheet)
                                  {
                                      styleElement.styleSheet.cssText = styleText;
                                  }
                              }";

        private void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            webView.InvokeScriptAsync("eval", new[] { DisableScrollingJs });
        }
    }
}
