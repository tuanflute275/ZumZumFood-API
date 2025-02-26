﻿namespace ZumZumFood.Application.Abstracts
{
    public interface IUserService
    {
        Task<ResponseObject> GetAllPaginationAsync(UserQuery userQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(UserModel model);
        Task<ResponseObject> UpdateAsync(int id, UserModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
