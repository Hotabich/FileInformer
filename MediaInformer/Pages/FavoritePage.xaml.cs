namespace MediaInformer.Pages
{
    using Windows.UI.Xaml.Controls;
    using ViewModels;
    public sealed partial class FavoritPage : Page
    {
        private readonly FavoriteViewModel favoriteViewModel = new FavoriteViewModel();
        public FavoritPage()
        {
            this.DataContext = this.favoriteViewModel;
            this.InitializeComponent();
        }
    }
}
