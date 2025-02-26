﻿namespace ZumZumFood.Domain.Abstracts
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        ITokenRepository TokenRepository { get; }
        IParameterRepository ParameterRepository { get; }
        ILogRepository LogRepository { get; }
        IBannerRepository BannerRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IBrandRepository BrandRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductDetailRepository ProductDetailRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IProductCommentRepository ProductCommentRepository { get; }
        IComboRepository ComboRepository { get; }
        IComboProductRepository ComboProductRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        ICouponRepository CouponRepository { get; }
        ICouponConditionRepository CouponConditionRepository { get; }
        ILocationRepository LocationRepository { get; }
        ICodeRepository CodeRepository { get; }
        ICodeValueRepository CodeValueRepository { get; }

        // other repository

        Task BeginTransaction();
        Task SaveChangeAsync();
        Task CommitTransactionAsync();
        void Dispose();
        Task RollbackTransactionAsync();
        DbSet<T> Table<T>() where T : class;
    }
}
