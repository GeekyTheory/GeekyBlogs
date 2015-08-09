using System;
using Windows.UI.Xaml.Media;

namespace GeekyBlogs.Models
{
    public class MenuItem
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public Type View { get; set; }
        public SolidColorBrush Brush { get; set; }
        public string Url { get; set; }
    }
}
