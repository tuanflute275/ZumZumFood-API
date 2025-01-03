namespace ZumZumFood.Application.Models.Request
{
    public class ParameterModel
    {
        public string ParaScope { get; set; }
        public string ParaName { get; set; }
        public string ParaType { get; set; }
        public string? ParaShortValue { get; set; }
        public string? ParaLobValue { get; set; }
        public string? ParaDesc { get; set; }
        public bool? UserAccessibleFlag { get; set; } // Người dùng có thể truy cập (true/false)
        public bool? AdminAccessibleFlag { get; set; }// Admin có thể truy cập (true/false)
    }
}
