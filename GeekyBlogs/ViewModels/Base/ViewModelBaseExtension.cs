using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GeekyBlogs.Models;
using GeekyTheory.ViewModels;

namespace GeekyBlogs.ViewModels.Base
{
    public abstract class ViewModelBaseExtension : ViewModelBase
    {
        private bool isPaneOpen;
        private ObservableCollection<MenuItem> menuItems;
        private MenuItem menuItem;

        public bool IsPaneOpen
        {
            get { return isPaneOpen; }
            set
            {
                if (isPaneOpen != value)
                {
                    isPaneOpen = value;
                    OnPropertyChanged();
                    GetCalculatedVariableSize(ViewWidth);
                }
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return menuItems; }
            set
            {
                if (menuItems != value)
                {
                    menuItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public MenuItem MenuItem
        {
            get { return menuItem; }
            set
            {
                if (menuItem != value)
                {
                    menuItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public void GetCalculatedVariableSize(double width)
        {
            if (ViewWidth < 641)
            {
                VariableSizedGrid_Width = width / 4;
            }
            else if (IsPaneOpen)
            {
                VariableSizedGrid_Width = (width - 60) / 4;
            }
            else
            {
                VariableSizedGrid_Width = (width - 60) / 4;
            }
        }

        public void AppView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewWidth = e.NewSize.Width;

            GetCalculatedVariableSize(ViewWidth);

        }
    }
}
