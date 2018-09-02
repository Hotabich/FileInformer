namespace MediaInformer.Popups
{
    using MediaInformer.ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class SetingsPopup : UserControl
    {
        private readonly SettingsPopupViewModel settingsPopupViewModel = new SettingsPopupViewModel();
        public SetingsPopup()
        {
            this.DataContext = this.settingsPopupViewModel;
            this.InitializeComponent();
        }
    }
}
