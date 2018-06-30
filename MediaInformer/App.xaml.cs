namespace MediaInformer
{
    using MediaInformer.DataAccets.Providers;
    using MediaInformer.Controls;
    using MediaInformer.DI;
    using MediaInformer.Models.Interfaces;
    using MediaInformer.Models.Enums;
    using MediaInformer.Pages;
    using System;
    using System.Collections.Generic;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.Foundation;
    using Windows.UI.Core;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.InitializeFactory();
            this.Suspending += OnSuspending;
        }

        private void InitializeFactory()
        {
            Factory.CommonFactory.Bind<INavigationProvider, NavigationProvider>(LifetimeMode.Singleton);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            this.InitializeWindowsApplication(e);
            base.OnLaunched(e);
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private async void InitializeWindowsApplication(IActivatedEventArgs args)
        {

            if (!(Window.Current.Content is RootControl rootControl))
            {
                var navigationProvider = Factory.CommonFactory.GetInstance<INavigationProvider>();
                rootControl = new RootControl();
                this.InitializeNavigationProvider(rootControl.RootFrame);
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    navigationProvider = Factory.CommonFactory.GetInstance<INavigationProvider>();
                    // await provider.RestoreHistory();
                }

            }
            Window.Current.Activated += OnCurrentActivated;
            Window.Current.Content = rootControl;

            Window.Current.Activate();
        }

        private static void OnCurrentActivated(object sender, WindowActivatedEventArgs e)
        {
            var appView = ApplicationView.GetForCurrentView();
            appView.SetPreferredMinSize(new Size(320, 320));
        }

        private void InitializeNavigationProvider(Frame frame)
        {
            var mapper = new Dictionary<Enum, Type>();
            mapper.Add(NavigationSource.MainPage, typeof(MainPage));
            mapper.Add(NavigationSource.RecentPage, typeof(RecentPage));

            var navigationProvider = Factory.CommonFactory.GetInstance<INavigationProvider>();
            navigationProvider.Initialize(frame, mapper);
            navigationProvider.Navigate(NavigationSource.MainPage);
        }
    }
}
