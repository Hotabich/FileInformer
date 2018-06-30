namespace MediaInformer.Helpers.TemplateSelectors
{
    using MediaInformer.Models;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class MediaToolItemTemplate : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }

        public DataTemplate EmptyTemplate { get; set; }

        public DataTemplate AddTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var resultTemplate = this.EmptyTemplate;

            var mediaItem = item as MediaToolItem;

            if (mediaItem != null)
            {
                if (mediaItem.IsEmpty)
                {
                    resultTemplate = this.AddTemplate;
                }
                else
                {
                    resultTemplate = this.FileTemplate;
                }
            }
            return resultTemplate;
        }
    }
}
