namespace ZumZumFood.Application.Services
{
    public class ComboService : IComboService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ComboService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ComboService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ComboService> logger, IHttpContextAccessor httpContextAccessor)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.ComboRepository.GetAllAsync(
                    expression: s => s.DeleteFlag != true && string.IsNullOrEmpty(keyword) 
                    || s.Name.Contains(keyword) 
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort`
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.ComboId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.ComboId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.ComboId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<ComboDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/combo", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/combo");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.ComboProductRepository.GetAllAsync(
                   expression: x => x.ComboId == id && x.Combo.DeleteFlag != true,
                   include: query => query.Include(x => x.Combo).Include(x => x.Product)
                );
                if (dataQuery == null || !(dataQuery.Count() > 0))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo/{id}", null, dataQuery.FirstOrDefault());
                    return new ResponseObject(404, "Combo not found.", dataQuery.FirstOrDefault());
                }
                var result = new List<ComboDTO>();
                var comboItem = dataQuery.FirstOrDefault();
                if (comboItem != null)
                {
                    var dto = new ComboDTO
                    {
                        ComboId = comboItem.ComboId,
                        Name = comboItem.Combo.Name,
                        Image = comboItem.Combo.Image,
                        Price = comboItem.Combo.Price,
                        Description = comboItem.Combo.Description,
                        IsActive = comboItem.Combo.IsActive,
                        CreateBy = comboItem.Combo.CreateBy,
                        CreateDate = comboItem.Combo.CreateDate.HasValue ? comboItem.Combo.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        UpdateBy = comboItem.Combo.UpdateBy,
                        UpdateDate = comboItem.Combo.UpdateDate.HasValue ? comboItem.Combo.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteBy = comboItem.Combo.DeleteBy,
                        DeleteDate = comboItem.Combo.DeleteDate.HasValue ? comboItem.Combo.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteFlag = comboItem.Combo.DeleteFlag,

                        // Lấy danh sách Products
                        Products = dataQuery
                        .Where(p => p.Product != null)
                        .Select(p => new ProductDTO
                        {
                            ProductId = p.Product.ProductId,
                            Name = p.Product.Name,
                            Slug = p.Product.Slug,
                            Image = p.Product.Image,
                            Price = p.Product.Price,
                            Discount = p.Product.Discount,
                            IsActive = p.Product.IsActive,
                            BrandId = p.Product.BrandId,
                            BrandName = p.Product.Brand != null ? p.Product.Brand.Name : null, // Kiểm tra Brand
                            Description = p.Product.Description,
                            CreateDate = p.Product.CreateDate.HasValue ? p.Product.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            UpdateBy = p.Product.UpdateBy,
                            UpdateDate = p.Product.UpdateDate.HasValue ? p.Product.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteBy = p.Product.DeleteBy,
                            DeleteDate = p.Product.DeleteDate.HasValue ? p.Product.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteFlag = p.Product.DeleteFlag,
                        })
                        .ToList()
                    };

                    result.Add(dto);
                }
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo/{id}", null, result);
                    return new ResponseObject(404, "Combo not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/combo/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/combo/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(ComboModel model)
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
                var combo = new Combo();
                combo.Name = model.Name;
                combo.Price = model.Price;
                combo.IsActive = model.IsActive;
                combo.Description = model.Description;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "combos");
                    combo.Image = image;
                }
                combo.CreateBy = model.CreateBy;
                combo.CreateDate = DateTime.Now;
                await _unitOfWork.ComboRepository.SaveOrUpdateAsync(combo);
                await _unitOfWork.SaveChangeAsync();
                var comboId = combo.ComboId;

                foreach(var productId in model.listProduct)
                {
                    var procheck = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
                    if (procheck != null)
                    {
                        var comboProduct = new ComboProduct
                        {
                            ComboId = comboId,
                            ProductId = productId
                        };
                        await _unitOfWork.ComboProductRepository.SaveOrUpdateAsync(comboProduct);
                        await _unitOfWork.SaveChangeAsync();
                    }
                }

                LogHelper.LogInformation(_logger, "POST", "/api/combo", model, combo);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/combo", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, ComboModel model)
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
                var combo = await _unitOfWork.ComboRepository.GetByIdAsync(id);
                if (combo == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/combo", null, $"Combo not found with id {id}");
                    return new ResponseObject(400, $"Combo not found with id {id}", null);
                }
                combo.Name = model.Name;
                combo.Price = model.Price;
                combo.IsActive = model.IsActive;
                combo.Description = model.Description;
                combo.UpdateBy = model.UpdateBy;
                combo.UpdateDate = DateTime.Now;
                await _unitOfWork.ComboRepository.SaveOrUpdateAsync(combo);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/combo", model, combo);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/combo", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var combo = await _unitOfWork.ComboRepository.GetByIdAsync(id);
                if (combo == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/combo/{id}", id, "Combo not found.");
                    return new ResponseObject(404, "Combo not found.", null);
                }
                combo.DeleteFlag = true;
                combo.DeleteBy = deleteBy;
                combo.DeleteDate = DateTime.Now;
                await _unitOfWork.ComboRepository.SaveOrUpdateAsync(combo);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/combo/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/combo/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.ComboRepository.GetAllAsync(x => x.DeleteFlag == true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted categories found." });
                    return new ResponseObject(404, "No deleted categories found.", null);
                }
                var data = _mapper.Map<List<ComboDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/combo/deleted-data", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var combo = await _unitOfWork.ComboRepository.GetByIdAsync(id);
                if (combo == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/combo/{id}/restore", new { id }, new { message = "Parameter not found." });
                    return new ResponseObject(404, "Combo not found.", null);
                }

                if ((bool)!combo.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/combo/{id}/restore", new { id }, new { message = "Parameter is not flagged as deleted." });
                    return new ResponseObject(400, "Combo is not flagged as deleted.", null);
                }

                combo.DeleteFlag = false;
                combo.DeleteBy = null;
                combo.DeleteDate = null;

                await _unitOfWork.ComboRepository.SaveOrUpdateAsync(combo);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/combo/{id}/restore", id, "Combo restored successfully");
                return new ResponseObject(200, "Combo restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/combo/{id}/restore", id);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/combo/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var combo = await _unitOfWork.ComboRepository.GetByIdAsync(id);
                if (combo == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/combo/{id}", id, "Combo not found.");
                    return new ResponseObject(404, "Combo not found.", null);
                }
                await _unitOfWork.ComboRepository.DeleteAsync(combo);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/combo/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/combo/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
