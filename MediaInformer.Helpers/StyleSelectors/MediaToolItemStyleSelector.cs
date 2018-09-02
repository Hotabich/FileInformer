namespace MediaInformer.Helpers.StyleSelectors
{
    using MediaInformer.Models;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class MediaToolItemStyleSelector : StyleSelector
    {
        public Style FileStyle { get; set; }

        public Style FileWithoutSelectStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var resultStyle = this.FileStyle;

            var mediaItem = item as MediaToolItem;

            if (mediaItem != null)
            {
                if (mediaItem.IsEmpty)
                {
                    resultStyle = FileWithoutSelectStyle;
                }
            }
            return resultStyle;
        }
    }
}
