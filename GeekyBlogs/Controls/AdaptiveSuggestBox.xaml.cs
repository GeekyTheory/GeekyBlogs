using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GeekyBlogs.Controls
{
    public sealed partial class AdaptiveSuggestBox : UserControl
    {
        private bool isOpened;

        public AdaptiveSuggestBox()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentWindowOnSizeChanged;
        }

        private void CurrentWindowOnSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            if (windowSizeChangedEventArgs.Size.Width <= 992)
            {
                isOpened = false;
            }
            else
            {
                isOpened = true;
            }
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Window.Current.Bounds.Width <= 992)
            {
                VisualStateManager.GoToState(this, !isOpened ? "LargeDevices" : "MediumDevices", true);
                isOpened = !isOpened;
            }
            else
            {
                isOpened = false;
            }
        }
    }
}
