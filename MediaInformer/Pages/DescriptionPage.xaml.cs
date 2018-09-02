namespace MediaInformer.Pages
{
    using MediaInformer.ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class DescriptionPage : Page
    {
        private readonly DescritpionViewModel descritpionViewModel = new DescritpionViewModel();
        public DescriptionPage()
        {
            this.DataContext = this.descritpionViewModel;
            this.InitializeComponent();
        }
    }
}
