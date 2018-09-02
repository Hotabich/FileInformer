namespace MediaInformer.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class AppBarSwitchButton : AppBarToggleButton
    {
        public static readonly DependencyProperty IconCheckedProperty = DependencyProperty.Register(
            "IconChecked",
            typeof(IconElement),
            typeof(AppBarSwitchButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IconUncheckedProperty = DependencyProperty.Register(
            "IconUnchecked",
            typeof(IconElement),
            typeof(AppBarSwitchButton),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LabelCheckedProperty = DependencyProperty.Register(
            "LabelChecked",
            typeof(string),
            typeof(AppBarSwitchButton),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty LabelUncheckedProperty = DependencyProperty.Register(
            "LabelUnchecked",
            typeof(string),
            typeof(AppBarSwitchButton),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ChangedPressingEnabledProperty = DependencyProperty.Register(
           "ChangedPressingEnabled",
           typeof(bool),
           typeof(AppBarSwitchButton),
           new PropertyMetadata(true));


        public AppBarSwitchButton()
        {
            this.DefaultStyleKey = typeof(AppBarSwitchButton);
        }

        public IconElement IconChecked
        {
            get { return (IconElement)this.GetValue(IconCheckedProperty); }
            set { this.SetValue(IconCheckedProperty, value); }
        }

        public IconElement IconUnchecked
        {
            get { return (IconElement)this.GetValue(IconUncheckedProperty); }
            set
            {
                this.SetValue(IconUncheckedProperty, value);
            }
        }

        public string LabelChecked
        {
            get { return (string)this.GetValue(LabelCheckedProperty); }
            set { this.SetValue(LabelCheckedProperty, value); }
        }

        public string LabelUnchecked
        {
            get { return (string)this.GetValue(LabelUncheckedProperty); }
            set
            {
                this.SetValue(LabelUncheckedProperty, value);
                this.Label = value;
            }
        }

        public bool ChangedPressingEnabled
        {
            get { return (bool)this.GetValue(ChangedPressingEnabledProperty); }
            set { this.SetValue(ChangedPressingEnabledProperty, value); }
        }

        protected override void OnToggle()
        {
            if (this.ChangedPressingEnabled)
            {
                this.IsChecked = !this.IsChecked;
                SetContent();
            }
        }

        private void SetContent()
        {
            if ((bool)this.IsChecked)
            {
                this.Icon = IconChecked;
                this.Label = LabelChecked;
            }
            else
            {
                this.Icon = IconUnchecked;
                this.Label = LabelUnchecked;
            }
        }

    }
}
