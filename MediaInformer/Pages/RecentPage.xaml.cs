
namespace MediaInformer.Pages
{
    using MediaInformer.ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class RecentPage : Page
    {
        private readonly RecentViewModel recentViewModel = new RecentViewModel();
        public RecentPage()
        {
            this.DataContext = this.recentViewModel;
            this.InitializeComponent();

        }
    }
}
