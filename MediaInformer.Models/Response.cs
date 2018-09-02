
namespace MediaInformer.Models
{
    public class Response
    {
        public string Result { get; set; }
        public Error Error { get; set; }
        public bool IsSuccess => Error == null ? true : false;
    }
}
