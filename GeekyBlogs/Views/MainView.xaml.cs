using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GeekyBlogs.ViewModels;
using GeekyTheory.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GeekyBlogs.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : PageBase
    {
        private MainViewModel vm;

        public MainView()
        {
            this.InitializeComponent();

            //base.SplitViewFrame = SplitViewFrame;

            vm = (MainViewModel)this.DataContext;
            vm.PropertyChanged += Vm_PropertyChanged;
            this.SizeChanged += MainView_SizeChanged;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPaneOpen")
            {
                GetCalculatedVariableSize(vm.ViewWidth, vm.ViewHeight);
            }
        }

        private void MainView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            vm.ViewWidth = e.NewSize.Width;
            vm.ViewHeight = e.NewSize.Height;

            GetCalculatedVariableSize(vm.ViewWidth, vm.ViewHeight);
            
        }

        private void GetCalculatedVariableSize(double width, double heigth)
        {
            if (vm.IsPaneOpen)
            {
                vm.VariableSizedGrid_Width = (width - 240)/4;
                //vm.VariableSizedGrid_Heigth = (heigth - 240)/4;
            }
            else
            {
                vm.VariableSizedGrid_Width = (width - 60)/4;
                //vm.VariableSizedGrid_Heigth = (heigth - 60)/4;
            }
        }
    }
}
