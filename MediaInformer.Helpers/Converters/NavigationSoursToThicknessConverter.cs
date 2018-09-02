
namespace MediaInformer.Helpers.Converters
{
    using System;
    using Models.Enums;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class NavigationSoursToThicknessConverter : IValueConverter
    {
        private const string InvertParameter = "invert";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = new Thickness(1);
            var currentPage = (NavigationSource)value;
            var page = (NavigationSource)parameter;
            if (currentPage == page)
            {
                result = new Thickness(5);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
