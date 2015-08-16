using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using GeekyBlogs.Models;
using GeekyBlogs.Views;

namespace GeekyBlogs.Services
{
    public interface ILoadSplitterMenuService
    {
        ObservableCollection<MenuItem> LoadMenu();
    }

    public class LoadSplitterMenuService : ILoadSplitterMenuService
    {
        public ObservableCollection<MenuItem> LoadMenu()
        {
            return new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Icons/Dashboard.png",
                    Title = "Portada",
                    Brush = "#212121",
                    View = typeof(MainView)
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Geeky/geeky_theory_icon_round.png",
                    Title = "Geeky Theory",
                    Brush = "#1ABB9C",
                    Url = "http://geekytheory.com/feed/",
                    View = typeof(MainView)
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Geeky/geeky_juegos_icon_round.png",
                    Title = "Geeky Juegos",
                    Brush = "#FF6C60",
                    Url = "http://geekyjuegos.com/feed/",
                    View = typeof(MainView)
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Icons/Category.png",
                    Title = "Categorías",
                }
            };
        }
    }
}
