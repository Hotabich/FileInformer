using System;
namespace MediaInformer.MediaTool
{
    using System;
    using Core;
    class Program
    {
        static void Main(string[] args)
        {
            var result = String.Empty;
            try
            {
                MediaTool mediaTool = new MediaTool(@"D:\Torrent\acdc-play-ball.mp4");
                result = mediaTool.GetProperties();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
