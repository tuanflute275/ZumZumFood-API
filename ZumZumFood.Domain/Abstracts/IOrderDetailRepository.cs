﻿using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync(Expression<Func<OrderDetail, bool>> expression = null,
           Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null);
        Task<OrderDetail?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(OrderDetail orderDetail);
        Task<bool> DeleteAsync(OrderDetail orderDetail);
    }
}
