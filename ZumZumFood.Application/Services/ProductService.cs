namespace ZumZumFood.Application.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisCacheService _redisCacheService;
        private readonly string _cacheKeyPrefix = "product_";
        public ProductService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<ProductService> logger, 
            IHttpContextAccessor httpContextAccessor,
            IRedisCacheService redisCacheService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _redisCacheService = redisCacheService;
        }

        public async Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.ValidateInput(keyword, sort, pageNo);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/product", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }

                // start cache
                var cacheKey = $"{_cacheKeyPrefix}{keyword ?? "all"}_{sort ?? "default"}_page_{pageNo}";
                var cachedData = await _redisCacheService.GetCacheAsync(cacheKey);
                if (cachedData != null)
                {
                    // Nếu có dữ liệu trong cache, trả về dữ liệu từ cache
                    var response = JsonConvert.DeserializeObject<ResponseCache>(cachedData);
                    return new ResponseObject(200, "Query data successfully", response);
                }
                //end cache

                var dataQuery = _unitOfWork.ProductRepository.GetAllAsync(
                    expression: s => s.DeleteFlag != true && string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)
                );
                var query = await dataQuery;

                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.ProductId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.ProductId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        case "Price-ASC":
                            query = query.OrderBy(x => x.Price);
                            break;
                        case "Price-DESC":
                            query = query.OrderByDescending(x => x.Price);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.ProductId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<ProductDTO>>(dataList);

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

                // Lưu vào Redis
                await _redisCacheService.SetCacheAsync(cacheKey, JsonConvert.SerializeObject(responseData), TimeSpan.FromHours(1));
                LogHelper.LogInformation(_logger, "GET", "/api/product", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/product");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.ProductRepository.GetAllAsync(
                   expression: x => x.ProductId == id && x.DeleteFlag != true,
                   include: query => query.Include(x => x.ProductDetails)
                                          .Include(x => x.ProductImages)
                                          .Include(x => x.ProductComments)
                                          .ThenInclude(pcm => pcm.User)
                                          .Include(x => x.Category)
                                          .Include(x => x.Brand)
                );
                var product = dataQuery.FirstOrDefault();
                if (product == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/product/{id}", null, product);
                    return new ResponseObject(404, "Product not found.", product);
                }
                var result = new ProductMapperDTO
                {
                    ProductId = product.ProductId,
                    Image = product.Image,
                    Name = product.Name,
                    Slug = product.Slug,
                    Price = product.Price,
                    Discount = product.Discount,
                    IsActive = product.IsActive,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    BrandId = product.BrandId,
                    BrandName = product.Brand.Name,
                    CreateBy = product.CreateBy,
                    CreateDate = product.CreateDate.HasValue ? product.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    UpdateBy = product.UpdateBy,
                    UpdateDate = product.UpdateDate.HasValue ? product.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteBy = product.DeleteBy,
                    DeleteDate = product.DeleteDate.HasValue ? product.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteFlag = product.DeleteFlag,
                    ProductImages = product.ProductImages.Select(d => new ProductImageDTO
                    {
                       ProductImageId = d.ProductId,
                       Path = d.Path
                    }).ToList(),
                    ProductComments = product.ProductComments.Select(c => new ProductCommentDTO
                    {
                        Email = c.Email,
                        Message = c.Message,
                        Name = c.Name,
                        Users = new UserMapperDTO
                        {
                            UserId = c.UserId,
                            UserName = c.User.UserName,
                            FullName = c.User.FullName,
                            Avatar = c.User.Avatar,
                            Email = c.User.Email,
                            Gender = c.User.Gender,
                            Address = c.User.Address,
                            PlaceOfBirth = c.User.PlaceOfBirth,
                            DateOfBirth = c.User.DateOfBirth.HasValue ? c.User.DateOfBirth.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            Nationality = c.User.Nationality,
                            PhoneNumber = c.User.PhoneNumber
                        }
                    }).ToList(),
                    ProductDetails = product.ProductDetails.Select(d => new ProductDetailDTO
                    {
                        Size = d.Size,
                    }).ToList()
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/product/{id}", null, result);
                    return new ResponseObject(404, "Product not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/product/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/product/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
        
        public async Task<ResponseObject> SaveAsync(ProductModel model)
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
                var product = new Product();
                product.Name = model.Name;
                product.Slug = Helpers.GenerateSlug(model.Name);
                product.Price = model.Price;
                product.Discount = model.Discount;
                product.IsActive = model.IsActive;
                var resCheck = await _unitOfWork.BrandRepository.GetByIdAsync(model.RestaurantId);
                if(resCheck != null)
                {
                    product.BrandId = model.RestaurantId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product", null, null);
                    return new ResponseObject(404, "Restaurant not found.", null);
                }
                var cateCheck = await _unitOfWork.CategoryRepository.GetByIdAsync(model.CategoryId);
                if (cateCheck != null)
                {
                    product.CategoryId = model.CategoryId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product", null, null);
                    return new ResponseObject(404, "Category not found.", null);
                }
                product.Description = model.Description;
                product.CreateBy = Constant.SYSADMIN;
                product.CreateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "products");
                    product.Image = image;
                }

                await _unitOfWork.ProductRepository.SaveOrUpdateAsync(product);
                await _unitOfWork.SaveChangeAsync();
                _redisCacheService.ClearCacheAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/product", model, product);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/product", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, ProductModel model)
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
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/product", null, $"Product not found with id {id}");
                    return new ResponseObject(400, $"Product not found with id {id}", null);
                }
                product.Name = model.Name;
                product.Slug = Helpers.GenerateSlug(model.Name);
                product.Price = model.Price;
                product.Discount = model.Discount;
                product.IsActive = model.IsActive;
                var resCheck = await _unitOfWork.BrandRepository.GetByIdAsync(model.RestaurantId);
                if (resCheck != null)
                {
                    product.BrandId = model.RestaurantId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product", null, null);
                    return new ResponseObject(404, "Restaurant not found.", null);
                }
                var cateCheck = await _unitOfWork.CategoryRepository.GetByIdAsync(model.CategoryId);
                if (cateCheck != null)
                {
                    product.CategoryId = model.CategoryId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product", null, null);
                    return new ResponseObject(404, "Category not found.", null);
                }
                product.Description = model.Description;
                product.UpdateBy = Constant.SYSADMIN;
                product.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "products");
                    product.Image = image;
                }

                await _unitOfWork.ProductRepository.SaveOrUpdateAsync(product);
                await _unitOfWork.SaveChangeAsync();
                _redisCacheService.ClearCacheAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/product", model, product);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/product", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/product/{id}", id, "Product not found.");
                    return new ResponseObject(404, "Product not found.", null);
                }
                product.DeleteFlag = true;
                product.DeleteBy = Constant.SYSADMIN;
                product.DeleteDate = DateTime.Now;
                await _unitOfWork.ProductRepository.SaveOrUpdateAsync(product);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/product/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/product/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.ProductRepository.GetAllAsync(x => x.DeleteFlag == true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted categories found." });
                    return new ResponseObject(404, "No deleted categories found.", null);
                }
                var data = _mapper.Map<List<ProductDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/product/deleted-data", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/product/{id}/restore", new { id }, new { message = "Product not found." });
                    return new ResponseObject(404, "Product not found.", null);
                }

                if ((bool)!product.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/product/{id}/restore", new { id }, new { message = "Product is not flagged as deleted." });
                    return new ResponseObject(400, "Product is not flagged as deleted.", null);
                }

                product.DeleteFlag = false;
                product.DeleteBy = null;
                product.DeleteDate = null;

                await _unitOfWork.ProductRepository.SaveOrUpdateAsync(product);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/product/{id}/restore", id, "Product restored successfully");
                return new ResponseObject(200, "Product restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/product/{id}/restore", id);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/product/{id}", id, "Product not found.");
                    return new ResponseObject(404, "Product not found.", null);
                }
                // Start: Deleting foreign key dependencies
                var productDetail = await _unitOfWork.ProductDetailRepository.GetAllAsync(x => x.ProductId == id);
                var productComment = await _unitOfWork.ProductCommentRepository.GetAllAsync(x => x.ProductId == id);
                var productImage = await _unitOfWork.ProductImageRepository.GetAllAsync(x => x.ProductId == id);
                var carts = await _unitOfWork.CartRepository.GetAllAsync(x => x.ProductId == id);
                var wishlists = await _unitOfWork.WishlistRepository.GetAllAsync(x => x.ProductId == id);
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync(x => x.ProductId == id);

                // Delete Product Details
                if (productDetail != null && productDetail.Any())  // Ensure there's data to delete
                {
                    await _unitOfWork.ProductDetailRepository.DeleteRangeAsync(productDetail.ToList());
                }

                // Delete Product Comments
                if (productComment != null && productComment.Any())  // Ensure there's data to delete
                {
                    await _unitOfWork.ProductCommentRepository.DeleteRangeAsync(productComment.ToList());
                }

                // Delete Product Images
                if (productImage != null && productImage.Any())  // Ensure there's data to delete
                {
                    await _unitOfWork.ProductImageRepository.DeleteRangeAsync(productImage.ToList());
                }

                // Delete carts
                if (carts != null && carts.Any())  // Ensure there's data to delete
                {
                    await _unitOfWork.CartRepository.DeleteRangeAsync(carts.ToList());
                }

                // Delete wishlists
                if (wishlists != null && wishlists.Any())  // Ensure there's data to delete
                {
                    await _unitOfWork.WishlistRepository.DeleteRangeAsync(wishlists.ToList());
                }

                // Delete orderDetails
                if (orderDetails != null && orderDetails.Any())  // Ensure there's data to delete
                {
                    await _unitOfWork.OrderDetailRepository.DeleteRangeAsync(orderDetails.ToList());
                }

                // Save changes after all deletions
                await _unitOfWork.SaveChangeAsync();
                // End: Deleting foreign key dependencies
                await _unitOfWork.ProductRepository.DeleteAsync(product);
                await _unitOfWork.SaveChangeAsync();
                _redisCacheService.ClearCacheAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/product/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/product/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
