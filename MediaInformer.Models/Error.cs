namespace MediaInformer.Models
{
    using Enums;
    public class Error
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public ErrorStatus Status { get; set; }

    }
}
