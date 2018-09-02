namespace MediaInformer.Helpers.Converters
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class BooleanToVisibilityConverter : IValueConverter
    {
        private const string InvertParameter = "invert";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleanValue = (bool)value;

            if ((string)parameter == InvertParameter)
            {
                booleanValue = !booleanValue;
            }

            return booleanValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
