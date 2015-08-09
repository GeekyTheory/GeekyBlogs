using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using GeekyBlogs.Models;

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
                    Icon = "ms-appx:///Assets/Icons/Back.png",
                    Title = "Atrás",
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Icons/Dashboard.png",
                    Title = "Portada",
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Geeky/geeky_theory_icon_round.png",
                    Title = "Geeky Theory",
                    Brush = (SolidColorBrush)Application.Current.Resources["GeekyTheoryColor"],
                    Url = "http://geekytheory.com/feed/"
                },
                new MenuItem
                {
                    Icon = "ms-appx:///Assets/Geeky/geeky_juegos_icon_round.png",
                    Title = "Geeky Juegos",
                    Brush = (SolidColorBrush)Application.Current.Resources["GeekyJuegosColor"],
                    Url = "http://geekyjuegos.com/feed/"
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
