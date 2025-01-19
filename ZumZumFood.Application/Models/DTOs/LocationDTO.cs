namespace ZumZumFood.Application.Models.DTOs
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int? Status { get; set; }
        public string? Name { get; set; }
        public string? NameEN { get; set; }
        public string? NameZH { get; set; }
        public int? ParentId { get; set; }
    }
}
