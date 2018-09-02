
namespace MediaInformer.Helpers.Behaviors
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml;
    using System.Windows.Input;
    using System;
    using Microsoft.Xaml.Interactivity;

    public class NavigationTrackerBehavior : Behavior<Page>
    {
        public static readonly DependencyProperty OnNavigationCommandProperty = DependencyProperty.Register(
        "OnNavigationCommand", typeof(ICommand), typeof(NavigationTrackerBehavior), new PropertyMetadata(null));

        public ICommand OnNavigationCommand
        {
            get { return (ICommand)GetValue(OnNavigationCommandProperty); }
            set { SetValue(OnNavigationCommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.OnNavigationCommand.Execute(null);
        }
    }
}
