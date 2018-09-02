namespace MediaInformer.Controls
{
    using Windows.UI.Xaml.Controls;
    public sealed partial class NavigationPanel : UserControl
    {
        public NavigationPanel()
        {
            this.InitializeComponent();
            this.DataContext = ViewModels.ViewModelsLocator.NavigationMenuViewModel;
        }
    }
}
