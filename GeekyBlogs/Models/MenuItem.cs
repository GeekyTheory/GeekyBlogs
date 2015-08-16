using System;
using System.Windows.Input;
using Windows.UI.Xaml.Media;

namespace GeekyBlogs.Models
{
    public class MenuItem : IMenuItem
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public Type View { get; set; }
        public ICommand Command { get; set; }
        public string Brush { get; set; }
        public string Url { get; set; }
    }
}
