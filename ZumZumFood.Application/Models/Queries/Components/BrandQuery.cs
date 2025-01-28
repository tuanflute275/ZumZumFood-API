using ZumZumFood.Application.Models.Queries.Base;

namespace ZumZumFood.Application.Models.Queries.Components
{
    public class BrandQuery : BaseQuery<Brand>
    {
        public string Name { get; set; }
    }
}
