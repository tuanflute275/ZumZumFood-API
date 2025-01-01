﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class CategoryService : ICategoryService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger, IHttpContextAccessor httpContextAccessor)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/category", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.CategoryRepository.GetAllAsync(
                    expression: s => s.DeleteFlag == false && string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.CategoryId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.CategoryId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.CategoryId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<CategoryDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/category", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/category");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/category/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.CategoryRepository.GetAllAsync(
                   expression: x => x.CategoryId == id && x.DeleteFlag == false
                   /*,include: query => query.Include(x => x.Products).ThenInclude(p => p.ProductDetails)
                   .Include(x => x.Products).ThenInclude(p => p.ProductComments)
                   .Include(x => x.Products).ThenInclude(p => p.ProductImages)*/
                );
                var result = _mapper.Map<CategoryDTO>(dataQuery.FirstOrDefault());
                LogHelper.LogInformation(_logger, "GET", "/api/category/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/category/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(CategoryRequestModel model)
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

                // mapper data
                var category = new Category();
                category.Name = model.Name;
                category.Slug = Helpers.GenerateSlug(model.Name);
                category.Description = model.Description;
                category.CreateBy = Constant.SYSADMIN;
                category.CreateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "categories");
                    category.Image = image;
                }

                await _unitOfWork.CategoryRepository.SaveOrUpdateAsync(category);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/category", model, category);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/category", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, CategoryRequestModel model)
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

                // mapper data
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                category.Name = model.Name;
                category.Slug = Helpers.GenerateSlug(model.Name);
                category.Description = model.Description;
                category.UpdateBy = Constant.SYSADMIN;
                category.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "categories");
                    category.Image = image;
                }

                await _unitOfWork.CategoryRepository.SaveOrUpdateAsync(category);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/category", model, category);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/category", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/category/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/category/{id}", id, "Category not found.");
                    return new ResponseObject(404, "Category not found.", null);
                }
                category.DeleteFlag = true;
                category.DeleteBy = Constant.SYSADMIN;
                category.DeleteDate = DateTime.Now;
                await _unitOfWork.CategoryRepository.SaveOrUpdateAsync(category);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/category/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/category/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.CategoryRepository.GetAllAsync(x => x.DeleteFlag == true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted categories found." });
                    return new ResponseObject(404, "No deleted categories found.", null);
                }
                var data = _mapper.Map<List<CategoryDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/category/deleted-data", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/category/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/category/{id}/restore", new { id }, new { message = "Category not found." });
                    return new ResponseObject(404, "Category not found.", null);
                }

                if ((bool)!category.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/category/{id}/restore", new { id }, new { message = "Category is not flagged as deleted." });
                    return new ResponseObject(400, "Category is not flagged as deleted.", null);
                }

                category.DeleteFlag = false;
                category.DeleteBy = null;
                category.DeleteDate = null;

                await _unitOfWork.CategoryRepository.SaveOrUpdateAsync(category);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/category/{id}/restore", id, "Category restored successfully");
                return new ResponseObject(200, "Category restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/category/{id}/restore", id);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/category/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/category/{id}", id, "Category not found.");
                    return new ResponseObject(404, "Category not found.", null);
                }
               /* //start delete foreign key
                var products = await _unitOfWork.ProductCommentRepository.GetAllAsync(x => x.UserId == );
                if (userRole != null)
                {
                    await _unitOfWork.UserRoleRepository.DeleteAsync(userRole);
                    await _unitOfWork.SaveChangeAsync();
                }
                //end delete foreign key*/
                await _unitOfWork.CategoryRepository.DeleteAsync(category);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/category/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/category/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
