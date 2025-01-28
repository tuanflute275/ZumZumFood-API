namespace ZumZumFood.Application.Services
{
    public class BrandService : IBrandService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BrandService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseObject> GetAllPaginationAsync(BrandQuery brandQuery)
        {
            var limit = brandQuery.PageSize > 0 ? brandQuery.PageSize : int.MaxValue;
            var start = brandQuery.PageNo > 0 ? (brandQuery.PageNo - 1) * limit : 0;
            try
            {
                var dataQuery = _unitOfWork.BrandRepository.GetAllAsync(
                    expression: x => x.DeleteFlag != true &&
                                     (string.IsNullOrEmpty(brandQuery.Name) || x.Name.Contains(brandQuery.Name))
                );
                var query = await dataQuery;

                // Áp dụng sắp xếp
                if (!string.IsNullOrEmpty(brandQuery.SortColumn))
                {
                    query = brandQuery.SortColumn switch
                    {
                        "Name" when brandQuery.SortAscending => query.OrderBy(x => x.Name),
                        "Name" when !brandQuery.SortAscending => query.OrderByDescending(x => x.Name),
                        "Id" when brandQuery.SortAscending => query.OrderBy(x => x.BrandId),
                        "Id" when !brandQuery.SortAscending => query.OrderByDescending(x => x.BrandId),
                        _ => query
                    };
                }
                else
                {
                    // Sắp xếp mặc định
                    query = query.OrderByDescending(x => x.BrandId);
                }

                // Get total count
                var totalCount = query.Count();

                // Apply pagination if SelectAll is false
                var pagedQuery = brandQuery.SelectAll
                    ? query.ToList()
                    : query
                        .Skip(start)
                        .Take(limit)
                        .ToList();

                // Map to DTOs
                var data = _mapper.Map<List<BrandDTO>>(pagedQuery);

                // Prepare response
                var responseData = new
                {
                    items = data,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / brandQuery.PageSize),
                    pageNumber = brandQuery.PageNo,
                    pageSize = brandQuery.PageSize
                };

                LogHelper.LogInformation(_logger, "GET", "/api/brand", $"Query: {JsonConvert.SerializeObject(brandQuery)}", data.Count);
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/brand");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/brand/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.BrandRepository.GetAllAsync(
                   expression: x => x.BrandId == id && x.DeleteFlag != true,
                   include: query => query.Include(x => x.Products)
                                            .ThenInclude(p => p.Category)
                );
                var brand = dataQuery.FirstOrDefault();
                if (brand == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/brand/{id}", "Brand not found.", null);
                    return new ResponseObject(404, "Brand not found.", brand);
                }
                var result = new BrandMapperDTO
                {
                    BrandId = brand.BrandId,
                    Name = brand.Name,
                    Image = brand.Image,
                    Address = brand.Address,
                    PhoneNumber = brand.PhoneNumber,
                    Email = brand.Email,
                    IsActive = brand.IsActive,
                    OpenTime = brand.OpenTime.Value.ToString(@"hh\:mm"),
                    CloseTime = brand.CloseTime.Value.ToString(@"hh\:mm"),
                    Description = brand.Description,
                    CreateBy = brand.CreateBy,
                    CreateDate = brand.CreateDate.HasValue ? brand.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    UpdateBy = brand.UpdateBy,
                    UpdateDate = brand.UpdateDate.HasValue ? brand.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteBy = brand.DeleteBy,
                    DeleteDate = brand.DeleteDate.HasValue ? brand.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteFlag = brand.DeleteFlag,
                    Products = brand.Products.Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Slug = p.Slug,
                        Image = p.Image,
                        Price = p.Price,
                        Discount = p.Discount,
                        IsActive = p.IsActive,
                        BrandId = p.BrandId,
                        BrandName = p.Brand.Name,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/brand/{id}", null, result);
                    return new ResponseObject(404, "Brand not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/brand/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/brand/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(BrandModel model)
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
                var brand = new Brand();
                brand.Name = model.Name;
                brand.Slug = Helpers.GenerateSlug(model.Name);
                brand.Address = model.Address;
                brand.PhoneNumber = model.PhoneNumber;
                brand.Email = model.Email;
                brand.IsActive = model.IsActive;
                brand.OpenTime = model.OpenTime != null ? TimeSpan.Parse(model.OpenTime) : null;
                brand.CloseTime = model.CloseTime != null ? TimeSpan.Parse(model.CloseTime) : null;
                brand.Description = model.Description;
                brand.CreateBy = model.CreateBy;
                brand.CreateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "brands");
                    brand.Image = image;
                }
                await _unitOfWork.BrandRepository.SaveOrUpdateAsync(brand);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/brand", model, brand);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/brand", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, BrandModel model)
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
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/brand", null, $"Brand not found with id {id}");
                    return new ResponseObject(400, $"Brand not found with id {id}", null);
                }
                brand.Name = model.Name;
                brand.Slug = Helpers.GenerateSlug(model.Name);
                brand.Address = model.Address;
                brand.PhoneNumber = model.PhoneNumber;
                brand.Email = model.Email;
                brand.IsActive = model.IsActive;
                brand.Description = model.Description;
                brand.UpdateBy = model.UpdateBy;
                brand.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "brands");
                    brand.Image = image;
                }
                await _unitOfWork.BrandRepository.SaveOrUpdateAsync(brand);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/brand", model, brand);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/brand", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/brand/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/brand/{id}", id, "Brand not found.");
                    return new ResponseObject(404, "Brand not found.", null);
                }
                brand.DeleteFlag = true;
                brand.DeleteBy = deleteBy;
                brand.DeleteDate = DateTime.Now;
                await _unitOfWork.BrandRepository.SaveOrUpdateAsync(brand);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/brand/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/brand/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.BrandRepository.GetAllAsync(x => x.DeleteFlag != true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted brand found." });
                    return new ResponseObject(404, "No deleted brand found.", null);
                }
                var data = _mapper.Map<List<BrandDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/brand/deleted-data", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/brand/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/brand/{id}/restore", new { id }, new { message = "Brand not found." });
                    return new ResponseObject(404, "Brand not found.", null);
                }

                if ((bool)!brand.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/brand/{id}/restore", new { id }, new { message = "Brand is not flagged as deleted." });
                    return new ResponseObject(400, "Brand is not flagged as deleted.", null);
                }

                brand.DeleteFlag = false;
                brand.DeleteBy = null;
                brand.DeleteDate = null;

                await _unitOfWork.BrandRepository.SaveOrUpdateAsync(brand);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/brand/{id}/restore", id, "Brand restored successfully");
                return new ResponseObject(200, "Brand restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/brand/{id}/restore", id);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/brand/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/brand/{id}", id, "Brand not found.");
                    return new ResponseObject(404, "Brand not found.", null);
                }
                // Bắt đầu: Xóa các phụ thuộc khóa ngoại
                var products = await _unitOfWork.ProductRepository.GetAllAsync(x => x.BrandId == id);
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
                await _unitOfWork.BrandRepository.DeleteAsync(brand);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/brand/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/brand/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
