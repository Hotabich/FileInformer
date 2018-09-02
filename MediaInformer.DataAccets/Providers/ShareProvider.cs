
namespace MediaInformer.DataAccets.Providers
{
    using System;
    using Models.AppManager;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.Foundation;


    public class ShareProvider
    {
        private string shareText;
        private static Lazy<ShareProvider> instance = new Lazy<ShareProvider>(() => new ShareProvider(), false);

        public static ShareProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public void Share()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);

            try
            {
                //if (!String.IsNullOrEmpty(bodyText))
                //{
                //    this.shareText = bodyText;
                //}
                DataTransferManager.ShowShareUI();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "Share Example";
            request.Data.Properties.Description = "A demonstration on how to share";
        }
    }
}
