namespace ZumZumFood.Application.Models.Queries.Components
{
    public class CategoryQuery : BaseQuery<Category>
    {
        public string Name { get; set; }
    }
}
