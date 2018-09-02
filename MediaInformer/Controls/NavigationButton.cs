namespace MediaInformer.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed class NavigationButton : Button
    {
        private static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof(string), typeof(NavigationButton), new PropertyMetadata(default(string)));

        private static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(string), typeof(NavigationButton), new PropertyMetadata(default(string)));

        public NavigationButton()
        {
            this.DefaultStyleKey = typeof(NavigationButton);
        }
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
