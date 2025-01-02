namespace ZumZumFood.Domain.Abstracts
{
    public interface ITokenRepository
    {
        Task<IEnumerable<Token>> GetAllAsync(Expression<Func<Token, bool>> expression = null,
           Func<IQueryable<Token>, IIncludableQueryable<Token, object>>? include = null);
        Task<Token?> GetByIdAsync(int id);
        Task<Token?> GetRefreshTokenAsync(string refreshToken);
        Task<bool> SaveOrUpdateAsync(Token token);
        Task<bool> DeleteAsync(Token token);
        Task<bool> DeleteRangeAsync(List<Token> listData);
    }
}
