namespace MediaInformer.MediaTool.Core
{
    using MediaInfoDotNet;
    public class MediaTool
    {
        private MediaFile mediaFile;
        public MediaTool()
        {
            this.mediaFile = new MediaFile("");
        }

        public MediaTool(string filePath)
        {
            this.mediaFile = new MediaFile(filePath);
        }

        public string GetProperties()
        {
            this.mediaFile.InformComplete = true;
            return this.mediaFile.Inform;
        }

        public string GetDescription()
        {
            return mediaFile.InfoParameters;
        }
    }
}
