namespace ZumZumFood.Application.Models.Queries.Components
{
    public class UserQuery : BaseQuery<User>
    {
        public string Keyword { get; set; }
    }
}
