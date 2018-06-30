namespace MediaInformer.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using System;

    public class BaseViewModel : ViewModelBase
    {
        private int busyCount;

        public BaseViewModel() { }

        protected int BusyCount
        {
            get
            {
                return this.busyCount;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Busy Count can't be negative value");
                }
                if (this.busyCount != value)
                {
                    this.busyCount = value;
                    this.RaisePropertyChanged(() => this.IsBusy);
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.BusyCount != 0;
            }
        }

    }
}
