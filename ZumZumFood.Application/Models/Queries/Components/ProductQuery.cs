namespace ZumZumFood.Application.Models.Queries.Components
{
    public class ProductQuery : BaseQuery<Product>
    {
        public string Name { get; set; }
    }
}
