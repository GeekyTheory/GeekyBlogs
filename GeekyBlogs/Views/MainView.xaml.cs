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
                GetCalculatedVariableSize(vm.ViewWidth);
            }
        }

        private void MainView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            var state = "OnehandState";
            if (e.NewSize.Width > 641)
                state = "DesktopState";

            VisualStateManager.GoToState(this, state, true);

            vm.ViewWidth = e.NewSize.Width;
            
            GetCalculatedVariableSize(vm.ViewWidth);
            
        }

        private void GetCalculatedVariableSize(double width)
        {
            if (vm.ViewWidth < 641)
            {
                vm.VariableSizedGrid_Width = width / 4;
            }
            else if (vm.IsPaneOpen)
            {
                vm.VariableSizedGrid_Width = (width - 240)/4;
            }
            else
            {
                vm.VariableSizedGrid_Width = (width - 60)/4;
            }
        }
    }
}
