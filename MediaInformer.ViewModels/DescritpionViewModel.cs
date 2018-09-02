
using MediaInformer.DataAccets.Providers;

namespace MediaInformer.ViewModels
{
    public class DescritpionViewModel : BaseViewModel
    {
        public DescritpionViewModel()
        {
            this.Initialize();
        }

        public string Description { get; set; }

        protected async override void Initialize()
        {
            this.BusyCount++;
            var response = await ServiceConnectionProvider.GetDescription();
            if (!string.IsNullOrEmpty(response.Result))
            {
                this.Description = response.Result;
                this.RaisePropertyChanged(() => this.Description);
            }
            this.BusyCount--;

        }
    }
}
