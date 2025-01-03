namespace ZumZumFood.Application.Models.Request
{
    public class LogRequestModel
    {
        public string? UserName { get; set; }
        public string? WorkTation { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? IpAdress { get; set; }
        public bool IsLogin { get; set; } = false;
    }
}
