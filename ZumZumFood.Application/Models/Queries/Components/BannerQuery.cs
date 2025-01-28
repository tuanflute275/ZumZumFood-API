using ZumZumFood.Application.Models.Queries.Base;

namespace ZumZumFood.Application.Models.Queries.Components
{
    public class BannerQuery : BaseQuery<Banner>
    {
        public string Name { get; set; }
    }
}
