
namespace MediaInformer.DataAccets.Providers
{
    using MediaInformer.Models;
    using System;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.AppService;
    using Windows.Foundation.Collections;
    using Newtonsoft.Json;

    public static class ServiceConnectionProvider
    {
        private const string FilePathKey = "filePath";
        private const string ModeKey = "mode";
        private const string InfoMode = "info";
        private const string DescriptionMode = "description";
        private const string ResponseKey = "response";

        private static AppServiceConnection connection;

        public static void InitializeTrackingProvider(AppServiceConnection appConnection)
        {
            connection = appConnection;
        }

        public static async Task<Response> GetFileInfo(string filePath)
        {
            ValueSet valueSet = new ValueSet
            {
                {FilePathKey, filePath },
                { ModeKey, InfoMode}
            };

            var result = await GetResponse(valueSet);

            return result;
        }

        public static async Task<Response> GetDescription()
        {
            ValueSet valueSet = new ValueSet
            {
                { ModeKey, DescriptionMode}
            };

            var result = await GetResponse(valueSet);

            return result;
        }

        private async static Task<Response> GetResponse(ValueSet keyValues)
        {
            var result = new Response();

            if (connection != null)
            {
                try
                {
                    AppServiceResponse response = await connection.SendMessageAsync(keyValues);
                    if (response.Status == AppServiceResponseStatus.Success)
                    {
                        var message = response.Message[ResponseKey];
                        result = JsonConvert.DeserializeObject<Response>(message.ToString());
                    }
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                }
            }
            return result;
        }
    }
}
