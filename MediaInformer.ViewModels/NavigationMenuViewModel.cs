namespace MediaInformer.ViewModels
{
    using System;
    using GalaSoft.MvvmLight.Command;
    using DI;
    using Models.Interfaces;
    using Models.Enums;
    using Windows.UI.Xaml.Navigation;

    public class NavigationMenuViewModel : BaseViewModel
    {
        private NavigationSource currentPage;

        public NavigationMenuViewModel()
        {
            this.NavigateToCommand = new RelayCommand<NavigationSource>(this.NavigateToExecute);
        }
        public NavigationSource CurrentPage
        {
            get { return this.currentPage; }
            set
            {
                if (this.currentPage != value)
                {
                    this.currentPage = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public RelayCommand<NavigationSource> NavigateToCommand { get; private set; }

        private void NavigateToExecute(NavigationSource page)
        {
            if (this.CurrentPage != page)
            {
                this.NavigationProvider.Navigate(page);
                this.CurrentPage = page;
            }
        }
    }
}
