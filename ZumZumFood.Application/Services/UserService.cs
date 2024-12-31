using AutoMapper;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Models.DTOs;
using ZumZumFood.Application.Models.RequestModel;
using ZumZumFood.Application.Models.Response;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Application.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Application.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1)
        {
            try
            {
                var userQuery = _unitOfWork.UserRepository.GetAllAsync(
                expression: s => s.DeleteFlag == false && string.IsNullOrEmpty(keyword) || s.UserName.Contains(keyword)
                || s.FullName.Contains(keyword),
                include: query => query.Include(x => x.UserRoles).ThenInclude(u => u.Role)
            );
                var users = await userQuery;
                var specialUsernames = new[] { "admin", "restaurantOwner", "restaurantStaff", "deliveryDriver", "user" };
                users = users.OrderBy(s => Array.IndexOf(specialUsernames, s.UserName) >= 0 ? Array.IndexOf(specialUsernames, s.UserName) : int.MaxValue)
                    .ThenBy(s => s.UserName); // Default sorting for others

                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            users = users.OrderBy(x => x.UserId);
                            break;
                        case "Id-DESC":
                            users = users.OrderByDescending(x => x.UserId);
                            break;
                        case "Name-ASC":
                            users = users.OrderBy(x => x.UserName);
                            break;
                        case "Name-DESC":
                            users = users.OrderByDescending(x => x.UserName);
                            break;
                        case "Email-ASC":
                            users = users.OrderBy(x => x.Email);
                            break;
                        case "Email-DESC":
                            users = users.OrderByDescending(x => x.Email);
                            break;
                        default:
                            // In case the sort string does not match any of the above, use default sorting
                            users = users.OrderBy(s => Array.IndexOf(specialUsernames, s.UserName) >= 0 ? Array.IndexOf(specialUsernames, s.UserName) : int.MaxValue)
                                         .ThenBy(s => s.UserName);
                            break;
                    }
                }

                // Map users to UserDTO
                var userList = users.ToList();
                var data = _mapper.Map<List<UserDTO>>(userList);

                // Paginate the result
                // Phân trang dữ liệu
                var pagedData = data.ToPagedList(pageNo, Constant.DEFAULT_PAGESIZE);

                // Return the paginated result in the response
                // Trả về kết quả phân trang bao gồm các thông tin phân trang
                // Create paginated response
                var responseData = new
                {
                    items = pagedData,                // Paginated items
                    totalCount = pagedData.TotalItemCount, // Total number of items
                    totalPages = pagedData.PageCount,      // Total number of pages
                    pageNumber = pagedData.PageNumber,     // Current page number
                    pageSize = pagedData.PageSize          // Page size
                };
                LogHelper.LogInformation(_logger, "GET", "/api/user", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/user");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetByIdAsync(int id)
        {
            try
            {
                var userQuery = await _unitOfWork.UserRepository.GetAllAsync(
                   expression: x => x.UserId == id && x.DeleteFlag == false,
                   include: query => query.Include(x => x.UserRoles).ThenInclude(u => u.Role)
                );
                var result = _mapper.Map<UserDTO>(userQuery.FirstOrDefault());
                LogHelper.LogInformation(_logger, "GET", "/api/user/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/user");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(UserRequestModel model)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return new ResponseObject(400, "Validation error", errorMessages);
                }
                // end validate

                var user = new User();
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, 12);
                user = _mapper.Map<User>(model);
                user.CreateBy = Constant.SYSADMIN;
                user.CreateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var avatar = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "users");
                    user.Avatar = avatar;
                }
                
                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/user", model, user);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/user", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, UserRequestModel model)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return new ResponseObject(400, "Validation error", errorMessages);
                }
                // end validate

                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, 12);
                user = _mapper.Map<User>(model);
                user.UpdateBy = Constant.SYSADMIN;
                user.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var avatar = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "users");
                    user.Avatar = avatar;
                }

                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/user", model, user);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/user", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteFlagAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/user/{id}", id, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Invalid ID. ID must be greater than 0.", null);
                }
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/user/{id}", id, "User not found.");
                    return new ResponseObject(404, "User not found.", null);
                }
                user.DeleteFlag = true;
                user.DeleteBy = Constant.SYSADMIN;
                user.DeleteDate = DateTime.Now;
                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/user/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/user/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedUsers = await _unitOfWork.UserRepository.GetAllAsync(user => user.DeleteFlag == true);
                if (deletedUsers == null || !deletedUsers.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-users", null, new { message = "No deleted users found." });
                    return new ResponseObject(404, "No deleted users found.", null);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-users", null, "Query user delete successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", deletedUsers);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/user/deleted-users", null);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> RestoreAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/user/{id}/restore", new { id }, new { message = "Invalid ID. ID must be greater than 0." });
                    return new ResponseObject(400, "Invalid ID. ID must be greater than 0.", null);
                }

                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/user/{id}/restore", new { id }, new { message = "User not found." });
                    return new ResponseObject(404, "User not found.", null);
                }

                if ((bool)!user.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/user/{id}/restore", new { id }, new { message = "User is not flagged as deleted." });
                    return new ResponseObject(400, "User is not flagged as deleted.", null);
                }

                user.DeleteFlag = false;
                user.DeleteBy = null;
                user.DeleteDate = null;

                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/user/{id}/restore", id, "User restored successfully");
                return new ResponseObject(200, "User restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/user/{id}/restore", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/user/{id}", id, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Invalid ID. ID must be greater than 0.", null);
                }
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/user/{id}", id, "User not found.");
                    return new ResponseObject(404, "User not found.", null);
                }
                //start delete foreign key
                var userRole = await _unitOfWork.UserRoleRepository.GetUserByIdAsync(id);
                if(userRole != null)
                {
                    await _unitOfWork.UserRoleRepository.DeleteAsync(userRole);
                    await _unitOfWork.SaveChangeAsync();    
                }
                //end delete foreign key
                await _unitOfWork.UserRepository.DeleteAsync(user);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/user/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/user/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
