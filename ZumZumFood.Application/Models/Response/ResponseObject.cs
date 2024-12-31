namespace ZumZumFood.Application.Models.Response
{
    public class ResponseObject
    {
        public int status { get; set; }
        public string message { get; set; }
        public object? data { get; set; }

        public ResponseObject(int code, string message)
        {
            status = code;
            this.message = message;
        }

        public ResponseObject(int code, string message, object? data)
        {
            status = code;
            this.message = message;
            this.data = data;
        }
    }
}
