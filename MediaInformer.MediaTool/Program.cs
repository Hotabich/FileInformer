using System;
namespace MediaInformer.MediaTool
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Core;
    using MediaInformer.MediaTool.Enum;
    using MediaInformer.MediaTool.Models;
    using Windows.ApplicationModel.AppService;
    using Windows.Foundation.Collections;
    using Newtonsoft.Json;
    using System.Text;
    using System.IO;

    class Program
    {
        private const string FilePathKey = "filePath";
        private const string ModeKey = "mode";
        private const string InfoMode = "info";
        private const string ResponseKey = "response";
        private const string DescriptionMode = "description";
        private const string FileNoExists = "File not found or file not exists";
        private const string NullArgument = "File path is null or empty";

        static AppServiceConnection connection = null;

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        static void Main(string[] args)
        {
            IntPtr hWnd = GetConsoleWindow();
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
            Thread appServiceThread = new Thread(new ThreadStart(ThreadProc));
            appServiceThread.Start();
            Console.ReadLine();
        }

        private static async void ThreadProc()
        {
            connection = new AppServiceConnection
            {
                AppServiceName = "CommunicationService",
                PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName
            };

            connection.RequestReceived += Connection_RequestReceived;

            var status = await connection.OpenAsync();
        }
        private static void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var mode = args.Request.Message[ModeKey].ToString();

            if (mode == InfoMode)
            {
                var filePath = args.Request.Message[FilePathKey].ToString();

                var response = GetInfo(filePath);
                SendResponse(response, args);
            }
            else if (mode == DescriptionMode)
            {
                var response = GetDescription();
                SendResponse(response, args);
            }
        }

        private static Response GetInfo(string filePath)
        {
            var response = new Response();
            if (String.IsNullOrEmpty(filePath))
            {
                var error = new Error
                {
                    Message = FileNoExists,
                    Status = ErrorStatus.FileNotFound
                };

                response.Error = error;

            }
            else if (!File.Exists(filePath))
            {
                var error = new Error
                {
                    Message = NullArgument,
                    Status = ErrorStatus.FileNotFound
                };

                response.Error = error;
            }
            else
            {
                try
                {
                    MediaTool mediaTool = new MediaTool(filePath);
                    var result = mediaTool.GetProperties();
                    response.Result = EncodedString(result);
                }
                catch (Exception ex)
                {
                    response.Error.Message = ex.Message;
                    response.Error.Code = ex.HResult;
                }
            }
            return response;
        }

        private static Response GetDescription()
        {
            var response = new Response();
            MediaTool mediaTool = new MediaTool();
            response.Result = mediaTool.GetDescription();
            return response;
        }

        private static string EncodedString(string sourceString)
        {
            var result = String.Empty;
            if (!String.IsNullOrEmpty(sourceString))
            {
                byte[] bytes = Encoding.Default.GetBytes(sourceString);
                result = Encoding.UTF8.GetString(bytes);
            }
            return result;
        }

        private static void SendResponse(Response response, AppServiceRequestReceivedEventArgs args)
        {
            string resultForResponce = String.Empty;
            if (response != null)
            {
                resultForResponce = JsonConvert.SerializeObject(response);
            }
            ValueSet valueSet = new ValueSet
            {
                { ResponseKey, resultForResponce }
            };
            try
            {
                args.Request.SendResponseAsync(valueSet).Completed += delegate { };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
