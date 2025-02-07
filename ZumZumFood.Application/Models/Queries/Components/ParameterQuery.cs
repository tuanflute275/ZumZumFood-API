namespace ZumZumFood.Application.Models.Queries.Components
{
    public class ParameterQuery : BaseQuery<Parameter>
    {
        public string Keyword { get; set; }
    }
}
