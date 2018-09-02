namespace MediaInformer.DataAccets.Providers
{
    using MediaInformer.Models.Enums;
    using MediaInformer.Models.Navigation.Peremeters;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class PopupProvider
    {
        private static readonly Lazy<PopupProvider> instance = new Lazy<PopupProvider>(() => new PopupProvider(), false);
        private readonly Dictionary<PopupNavigationSource, Type> popupContentTypeMapping = new Dictionary<PopupNavigationSource, Type>();

        public event EventHandler<PopupChangedEventArgs> PopupChanged;

        public static PopupProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public PopupNavigationSource CurrentPopupNavigationSource { get; private set; }

        public void AddMapping(PopupNavigationSource source, Type elementType)
        {
            if (!this.popupContentTypeMapping.ContainsKey(source))
            {
                this.popupContentTypeMapping.Add(source, elementType);
            }
        }

        public void ShowPopupContent(PopupNavigationSource source)
        {
            if (this.CurrentPopupNavigationSource != source)
            {
                this.CurrentPopupNavigationSource = source;

                var handler = Volatile.Read(ref this.PopupChanged);

                if (handler != null)
                {
                    var type = this.popupContentTypeMapping[source];
                    handler.Invoke(this, new PopupChangedEventArgs(type, source));
                }
            }
        }

        public void CloseAnyPopup()
        {
            this.ShowPopupContent(PopupNavigationSource.CloseAnyPopup);
        }
    }
}
