namespace MediaInformer.Models.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;

    public interface INavigationProvider
    {
        void GoBack();
        void GoForward();
        bool CanGoBack { get; }
        Frame Frame { get; set; }
        bool CanGoForward { get; }
        void Navigate(Enum sourcePage);
        void CheckBackBattonVisibility();
        void Navigate(Enum sourcePage, object parameter);
        void Initialize(Frame frame, Dictionary<Enum, Type> mapper);
    }
}
