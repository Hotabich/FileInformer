namespace MediaInformer.MediaTool.Models
{
    using Interfaces;
    public class Error
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public ErrorStatus Status { get; set; }
    }
}
