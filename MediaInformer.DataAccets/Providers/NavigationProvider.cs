namespace MediaInformer.DataAccets.Providers
{
    using System;
    using System.Collections.Generic;
    using MediaInformer.Models.Interfaces;
    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;

    public class NavigationProvider : INavigationProvider
    {
        private Dictionary<Enum, Type> navigationMapper;
        private readonly SystemNavigationManager navigationManager = SystemNavigationManager.GetForCurrentView();
        private static Lazy<NavigationProvider> instance = new Lazy<NavigationProvider>(() => new NavigationProvider(), false);

        public NavigationProvider Instance => instance.Value;

        public Frame Frame { get; set; }

        public bool CanGoBack => Frame.CanGoBack;

        public bool CanGoForward => Frame.CanGoForward;

        public void Initialize(Frame frame, Dictionary<Enum, Type> mapper)
        {
            Frame = frame;
            navigationMapper = mapper;
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public void GoForward()
        {
            if (CanGoForward)
            {
                Frame.GoForward();
            }
        }

        public void Navigate(Enum sourcePage)
        {
            if (this.navigationMapper.ContainsKey(sourcePage))
            {
                Frame.Navigate(navigationMapper[sourcePage]);
            }
            this.CheckBackBattonVisibility();
        }

        public void Navigate(Enum sourcePage, object parameter)
        {
            if (this.navigationMapper.ContainsKey(sourcePage))
            {
                Frame.Navigate(navigationMapper[sourcePage], parameter);
            }
            this.CheckBackBattonVisibility();
        }

        public void CheckBackBattonVisibility()
        {
            this.navigationManager.AppViewBackButtonVisibility = this.CanGoBack
               ? AppViewBackButtonVisibility.Visible
               : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
