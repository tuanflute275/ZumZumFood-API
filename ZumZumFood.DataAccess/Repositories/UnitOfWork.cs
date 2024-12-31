using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private bool disposedValue;
        ApplicationDbContext _context;
        IDbContextTransaction _dbContextTransaction;
        // DI repository
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;
        IUserRoleRepository _userRoleRepository;
        ITokenRepository _tokenRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table<T>() where T : class => _context.Set<T>();

        // DI repository
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);
        public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new UserRoleRepository(_context);
        public ITokenRepository TokenRepository => _tokenRepository ??= new TokenRepository(_context);
      

        public async Task BeginTransaction()
        {
            _dbContextTransaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContextTransaction.RollbackAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
