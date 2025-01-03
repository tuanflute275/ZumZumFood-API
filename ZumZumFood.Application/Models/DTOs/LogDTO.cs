namespace ZumZumFood.Application.Models.DTOs
{
    public class LogDTO
    {
        public int LogId { get; set; }
        public string? UserName { get; set; }
        public string? WorkTation { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? IpAdress { get; set; }
        public string? TimeLogin { get; set; }
        public string? TimeLogout { get; set; }
        public string? TimeActionRequest { get; set; }
    }
}
