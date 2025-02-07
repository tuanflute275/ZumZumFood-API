namespace ZumZumFood.Application.Models.Queries.Components
{
    public class OrderQuery : BaseQuery<Domain.Entities.Order>
    {
        public string Keyword { get; set; }
    }
}
