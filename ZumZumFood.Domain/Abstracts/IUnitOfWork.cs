using Microsoft.EntityFrameworkCore;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        ITokenRepository TokenRepository { get; }
        // other repository

        Task BeginTransaction();
        Task SaveChangeAsync();
        Task CommitTransactionAsync();
        void Dispose();
        Task RollbackTransactionAsync();
        DbSet<T> Table<T>() where T : class;
    }
}
