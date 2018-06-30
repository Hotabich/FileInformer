namespace MediaInformer.Pages
{
    using MediaInformer.DI;
    using MediaInformer.Models.Enums;
    using MediaInformer.Models.Interfaces;
    using MediaInformer.ViewModels;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel mainPageViewModel = new MainPageViewModel();
        public MainPage()
        {
            this.DataContext = mainPageViewModel;
            this.InitializeComponent();
        }
    }
}
