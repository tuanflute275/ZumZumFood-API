namespace ZumZumFood.Application.Models.Queries.Components
{
    public class LogQuery : BaseQuery<Log>
    {
        public string Keyword { get; set; }
    }
}
