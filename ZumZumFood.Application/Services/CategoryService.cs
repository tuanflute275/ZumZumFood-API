using ZumZumFood.Application.Utils.Common;
using ZumZumFood.Application.Utils.Helpers;

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
                    expression: s => s.DeleteFlag != true && string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)
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
                   expression: x => x.CategoryId == id && x.DeleteFlag != true,
                   include: query => query.Include(x => x.Products)
                                            .ThenInclude(p => p.Brand)
                );
                var category = dataQuery.FirstOrDefault();
                if (category == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/category/{id}", null, category);
                    return new ResponseObject(404, "Category not found.", category);
                }
                var result = new CategoryMapperDTO
                {
                    CategoryId = category.CategoryId,
                    Image = category.Image,
                    Name = category.Name,
                    Slug = category.Slug,
                    IsActive = category.IsActive,
                    CreateBy = category.CreateBy,
                    CreateDate = category.CreateDate.HasValue ? category.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    UpdateBy = category.UpdateBy,
                    UpdateDate = category.UpdateDate.HasValue ? category.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteBy = category.DeleteBy,
                    DeleteDate = category.DeleteDate.HasValue ? category.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteFlag = category.DeleteFlag,
                    Products = category.Products.Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Slug = p.Slug,
                        Image = p.Image,
                        Price = p.Price,
                        Discount = p.Discount,
                        IsActive= p.IsActive,
                        BrandId = p.BrandId,
                        BrandName = p.Brand.Name,
                        Description = p.Description,
                        CreateDate = p.CreateDate.HasValue ? p.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        UpdateBy = p.UpdateBy,
                        UpdateDate = p.UpdateDate.HasValue ? p.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteBy = p.DeleteBy,
                        DeleteDate = p.DeleteDate.HasValue ? p.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteFlag = p.DeleteFlag,
                    }).ToList()
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/category/{id}", null, result);
                    return new ResponseObject(404, "Category not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/category/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/category/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(CategoryModel model)
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
                category.CreateBy = model.CreateBy;
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

        public async Task<ResponseObject> UpdateAsync(int id, CategoryModel model)
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
                if (category == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/category", null, $"Category not found with id {id}");
                    return new ResponseObject(400, $"Category not found with id {id}", null);
                }
                category.Name = model.Name;
                category.Slug = Helpers.GenerateSlug(model.Name);
                category.Description = model.Description;
                category.UpdateBy = model.UpdateBy;
                category.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "categories");
                    category.Image = image;
                }

                await _unitOfWork.CategoryRepository.SaveOrUpdateAsync(category);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/category", model, category);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/category", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy)
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
                category.DeleteBy = deleteBy;
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
                // Bắt đầu: Xóa các phụ thuộc khóa ngoại
                var products = await _unitOfWork.ProductRepository.GetAllAsync(x => x.CategoryId == id);
                if (products != null && products.Any())
                {
                    // Lấy tất cả dữ liệu liên quan cùng lúc, giảm số lượng truy vấn
                    var productDetails = await _unitOfWork.ProductDetailRepository.GetAllAsync(x => products.Select(p => p.ProductId).Contains(x.ProductId));
                    var productComments = await _unitOfWork.ProductCommentRepository.GetAllAsync(x => products.Select(p => p.ProductId).Contains(x.ProductId));
                    var productImages = await _unitOfWork.ProductImageRepository.GetAllAsync(x => products.Select(p => p.ProductId).Contains(x.ProductId));

                    // Xóa Product Details
                    if (productDetails?.Any() == true)
                    {
                        await _unitOfWork.ProductDetailRepository.DeleteRangeAsync(productDetails.ToList());
                    }

                    // Xóa Product Comments
                    if (productComments?.Any() == true)
                    {
                        await _unitOfWork.ProductCommentRepository.DeleteRangeAsync(productComments.ToList());
                    }

                    // Xóa Product Images
                    if (productImages?.Any() == true)
                    {
                        await _unitOfWork.ProductImageRepository.DeleteRangeAsync(productImages.ToList());
                    }

                    // Xóa Products
                    await _unitOfWork.ProductRepository.DeleteRangeAsync(products.ToList());
                }

                // Lưu các thay đổi sau khi xóa tất cả
                await _unitOfWork.SaveChangeAsync();
                // Kết thúc: Xóa các phụ thuộc khóa ngoại

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
