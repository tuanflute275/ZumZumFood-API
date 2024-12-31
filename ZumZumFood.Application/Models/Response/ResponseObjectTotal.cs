namespace ZumZumFood.Application.Models.Response
{
    public class ResponseObjectTotal
    {
        public int status { get; set; }
        public string message { get; set; }
        public object? total { get; set; }
        public object? data { get; set; }

        public ResponseObjectTotal(int code, string message, object? total, object? data)
        {
            status = code;
            this.message = message;
            this.total = total;
            this.data = data;
        }
    }
}
