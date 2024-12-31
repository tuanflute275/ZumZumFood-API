using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class TokenRepository : BaseRepository<Token>, ITokenRepository
    {
        public TokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Token>> GetAllAsync(Expression<Func<Token, bool>> expression = null,
          Func<IQueryable<Token>, IIncludableQueryable<Token, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Token?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.Id == id);
        }

        public async Task<Token?> GetRefreshTokenAsync(string refreshToken)
        {
            return await base.GetSingleAsync(x => x != null && x.RefreshToken.Contains(refreshToken));
        }

        public async Task<bool> SaveOrUpdateAsync(Token token)
        {
            try
            {
                if (token.Id == 0)
                {
                    await base.AddAsync(token);
                }
                else
                {
                    base.UpdateAsync(token);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Token token)
        {
            try
            {
                base.DeleteAsync(token);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
