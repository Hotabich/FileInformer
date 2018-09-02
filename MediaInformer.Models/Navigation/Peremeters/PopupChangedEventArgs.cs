namespace MediaInformer.Models.Navigation.Peremeters
{
    using MediaInformer.Models.Enums;
    using System;

    public class PopupChangedEventArgs : EventArgs
    {
        public PopupChangedEventArgs(Type type, PopupNavigationSource popupNavigationSource)
        {
            this.Type = type;
            this.PopupNavigationSource = popupNavigationSource;
        }
        public PopupNavigationSource PopupNavigationSource { get; private set; }

        public Type Type { get; private set; }
    }
}
