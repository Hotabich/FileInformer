namespace MediaInformer.Pages
{
    using ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class InfoPage : Page
    {
        private readonly InfoPageViewModel infoPageViewModel = new InfoPageViewModel();
        public InfoPage()
        {
            this.DataContext = this.infoPageViewModel;
            this.InitializeComponent();
        }
    }
}
