namespace MediaInformer.Controls
{
    using System;
    using MediaInformer.DataAccets.Providers;
    using MediaInformer.DI;
    using MediaInformer.Models.Enums;
    using MediaInformer.Models.Interfaces;
    using MediaInformer.Models.Navigation.Peremeters;
    using MediaInformer.Popups;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    public sealed partial class RootControl : UserControl
    {
        private bool alreadyCleanedUp;
        private readonly SystemNavigationManager navigationManager = SystemNavigationManager.GetForCurrentView();
        private INavigationProvider navigationProvider = Factory.CommonFactory.GetInstance<INavigationProvider>();


        public RootControl()
        {
            this.InitializeComponent();
            this.navigationManager.BackRequested += this.OnBackRequested;
            PopupProvider.Instance.PopupChanged += this.OnPopupChanged;
            this.InitializePopup();
        }

        public Frame RootFrame
        {
            get
            {
                return this.rootFrame;
            }
        }


        private void OnPopupChanged(object sender, PopupChangedEventArgs e)
        {
            if (e?.Type != null)
            {
                this.alreadyCleanedUp = false;
                PopupContentControl.Content = Activator.CreateInstance(e.Type);
                this.PopupContentBackground.Visibility = PopupContentControl.Visibility = Visibility.Visible;
                this.PopupContentBackground.Tapped -= this.OnPopupContentBackgroundTapped;
                this.PopupContentBackground.Tapped += this.OnPopupContentBackgroundTapped;
            }
            else if (!this.alreadyCleanedUp)
            {
                this.CleanAndClose();
            }
        }

        private void OnPopupContentBackgroundTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.alreadyCleanedUp)
            {
                this.CleanAndClose();
            }
        }

        private void CleanAndClose()
        {
            this.alreadyCleanedUp = true;
            PopupProvider.Instance.ShowPopupContent(PopupNavigationSource.CloseAnyPopup);
            this.PopupContentBackground.Visibility = PopupContentControl.Visibility = Visibility.Collapsed;
            this.PopupContentBackground.Tapped -= this.OnPopupContentBackgroundTapped;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = this.RequestBack();
        }

        private void InitializePopup()
        {
            PopupProvider.Instance.AddMapping(PopupNavigationSource.SettingsPopup, typeof(SetingsPopup));
            PopupProvider.Instance.AddMapping(PopupNavigationSource.CloseAnyPopup, null);
            PopupProvider.Instance.ShowPopupContent(PopupNavigationSource.CloseAnyPopup);
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
