namespace MediaInformer.Helpers.Extensions
{
    using Windows.UI.Xaml;
    public class MultiVisibilityExtension
    {

        public static readonly DependencyProperty FirstDependencyContentProperty = DependencyProperty.RegisterAttached("FirstDependencyContent", typeof(object), typeof(MultiVisibilityExtension), new PropertyMetadata(default(object), OnPropertyChanged));


        public static readonly DependencyProperty SecondDependencyContentProperty = DependencyProperty.RegisterAttached("SecondDependencyContent", typeof(object), typeof(MultiVisibilityExtension), new PropertyMetadata(default(object), OnPropertyChanged));


        public static readonly DependencyProperty ConditionModeProperty = DependencyProperty.RegisterAttached("ConditionMode", typeof(ConditionMode), typeof(MultiVisibilityExtension), new PropertyMetadata(default(ConditionMode)));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element != null)
            {
                ChangeVisibility(element);
            }
        }

        private static void ChangeVisibility(UIElement element)
        {
            var conditionMode = GetConditionMode(element);

            var firstConditionValue = GetFirstDependencyContent(element);
            var secondConditionValue = GetSecondDependencyContent(element);

            var firstConditionResult = false;
            var secondConditionResult = false;

            if (firstConditionValue != null)
            {
                firstConditionResult = (Visibility)firstConditionValue == Visibility.Visible;
            }

            if (secondConditionValue != null)
            {
                secondConditionResult = (Visibility)secondConditionValue == Visibility.Visible;
            }

            var conditionResult = conditionMode == ConditionMode.And ? firstConditionResult && secondConditionResult : firstConditionResult || secondConditionResult;

            element.Visibility = conditionResult ? Visibility.Visible : Visibility.Collapsed;
        }

        public static ConditionMode GetConditionMode(DependencyObject obj)
        {
            return (ConditionMode)obj.GetValue(ConditionModeProperty);
        }

        public static void SetConditionMode(DependencyObject obj, ConditionMode value)
        {
            obj.SetValue(ConditionModeProperty, value);
        }

        public static object GetSecondDependencyContent(DependencyObject obj)
        {
            return (object)obj.GetValue(SecondDependencyContentProperty);
        }

        public static void SetSecondDependencyContent(DependencyObject obj, object value)
        {
            obj.SetValue(SecondDependencyContentProperty, value);
        }

        public static object GetFirstDependencyContent(DependencyObject obj)
        {
            return (object)obj.GetValue(FirstDependencyContentProperty);
        }

        public static void SetFirstDependencyContent(DependencyObject obj, object value)
        {
            obj.SetValue(FirstDependencyContentProperty, value);
        }
    }

    public enum ConditionMode
    {
        And,
        Or
    }
}

