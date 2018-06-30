namespace MediaInformer.Controls
{
    using MediaInformer.DI;
    using MediaInformer.Models.Interfaces;
    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;

    public sealed partial class RootControl : UserControl
    {
        private readonly SystemNavigationManager navigationManager = SystemNavigationManager.GetForCurrentView();
        private INavigationProvider navigationProvider = Factory.CommonFactory.GetInstance<INavigationProvider>();
        public RootControl()
        {
            this.InitializeComponent();
            this.navigationManager.BackRequested += this.OnBackRequested;
        }

        public Frame RootFrame
        {
            get
            {
                return this.rootFrame;
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = this.RequestBack();
        }

        private bool RequestBack()
        {
            var isRequested = true;

            if (this.navigationProvider.CanGoBack)
            {
                this.navigationProvider.GoBack();
            }
            else
            {
                isRequested = false;
            }

            this.navigationProvider.CheckBackBattonVisibility();

            return isRequested;
        }
    }
}
