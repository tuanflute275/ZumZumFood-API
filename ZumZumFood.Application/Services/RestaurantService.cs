using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using X.PagedList;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Models.DTOs;
using ZumZumFood.Application.Models.Request;
using ZumZumFood.Application.Models.Response;
using ZumZumFood.Application.Utils;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using static ZumZumFood.Application.Utils.Helpers;

namespace ZumZumFood.Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RestaurantService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RestaurantService> logger, IHttpContextAccessor httpContextAccessor)
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
                // validate invalid special characters
                var validationResult = InputValidator.ValidateInput(keyword, sort, pageNo);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/restaurant", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.RestaurantRepository.GetAllAsync(
                    expression: s => s.DeleteFlag == false && string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.RestaurantId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.RestaurantId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.RestaurantId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<RestaurantDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/restaurant", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/restaurant");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetByIdAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/restaurant/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.RestaurantRepository.GetAllAsync(
                   expression: x => x.RestaurantId == id && x.DeleteFlag == false
                   /*,include: query => query.Include(x => x.Products).ThenInclude(p => p.ProductDetails)
                   .Include(x => x.Products).ThenInclude(p => p.ProductComments)
                   .Include(x => x.Products).ThenInclude(p => p.ProductImages)*/
                );
                var result = _mapper.Map<RestaurantDTO>(dataQuery.FirstOrDefault());
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/restaurant/{id}", null, result);
                    return new ResponseObject(404, "Restaurant not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", "/api/restaurant/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/restaurant/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(RestaurantRequestModel model)
        {
            try
            {
                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return new ResponseObject(400, "Validation error", errorMessages);
                }
                // end validate

                // mapper data
                var restaurant = new Restaurant();
                restaurant.Name = model.Name;
                restaurant.Slug = Helpers.GenerateSlug(model.Name);
                restaurant.Address = model.Address;
                restaurant.PhoneNumber = model.PhoneNumber;
                restaurant.Email = model.Email;
                restaurant.IsActive = model.IsActive;
                restaurant.Description = model.Description;
                restaurant.CreateBy = Constant.SYSADMIN;
                restaurant.CreateDate = DateTime.Now;
                
                await _unitOfWork.RestaurantRepository.SaveOrUpdateAsync(restaurant);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/restaurant", model, restaurant);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/restaurant", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, RestaurantRequestModel model)
        {
            try
            {
                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return new ResponseObject(400, "Validation error", errorMessages);
                }
                // end validate

                // mapper data
                var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(id);
                restaurant.Name = model.Name;
                restaurant.Slug = Helpers.GenerateSlug(model.Name);
                restaurant.Address = model.Address;
                restaurant.PhoneNumber = model.PhoneNumber;
                restaurant.Email = model.Email;
                restaurant.IsActive = model.IsActive;
                restaurant.Description = model.Description;
                restaurant.UpdateBy = Constant.SYSADMIN;
                restaurant.UpdateDate = DateTime.Now;
              
                await _unitOfWork.RestaurantRepository.SaveOrUpdateAsync(restaurant);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/restaurant", model, restaurant);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/restaurant", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteFlagAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/restaurant/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(id);
                if (restaurant == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/restaurant/{id}", id, "Restaurant not found.");
                    return new ResponseObject(404, "Restaurant not found.", null);
                }
                restaurant.DeleteFlag = true;
                restaurant.DeleteBy = Constant.SYSADMIN;
                restaurant.DeleteDate = DateTime.Now;
                await _unitOfWork.RestaurantRepository.SaveOrUpdateAsync(restaurant);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/restaurant/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/restaurant/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.RestaurantRepository.GetAllAsync(x => x.DeleteFlag == true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted restaurants found." });
                    return new ResponseObject(404, "No deleted categories found.", null);
                }
                var data = _mapper.Map<List<RestaurantDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/restaurant/deleted-data", null);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> RestoreAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/restaurant/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(id);
                if (restaurant == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/restaurant/{id}/restore", new { id }, new { message = "Restaurant not found." });
                    return new ResponseObject(404, "Restaurant not found.", null);
                }

                if ((bool)!restaurant.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/restaurant/{id}/restore", new { id }, new { message = "Restaurant is not flagged as deleted." });
                    return new ResponseObject(400, "Restaurant is not flagged as deleted.", null);
                }

                restaurant.DeleteFlag = false;
                restaurant.DeleteBy = null;
                restaurant.DeleteDate = null;

                await _unitOfWork.RestaurantRepository.SaveOrUpdateAsync(restaurant);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/restaurant/{id}/restore", id, "Restaurant restored successfully");
                return new ResponseObject(200, "Restaurant restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/restaurant/{id}/restore", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/restaurant/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(id);
                if (restaurant == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/restaurant/{id}", id, "Restaurant not found.");
                    return new ResponseObject(404, "Restaurant not found.", null);
                }
               /* //start delete foreign key
                var products = await _unitOfWork.ProductCommentRepository.GetAllAsync(x => x.UserId == );
                if (userRole != null)
                {
                    await _unitOfWork.UserRoleRepository.DeleteAsync(userRole);
                    await _unitOfWork.SaveChangeAsync();
                }
                //end delete foreign key*/
                await _unitOfWork.RestaurantRepository.DeleteAsync(restaurant);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/restaurant/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/restaurant/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
