namespace ZumZumFood.Application.Services
{
    public class WishlistService : IWishlistService
    {
        IUnitOfWork _unitOfWork;
        private readonly ILogger<WishlistService> _logger;
        public WishlistService(IUnitOfWork unitOfWork,ILogger<WishlistService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseObject> GetByIdAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/wishlist/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.WishlistRepository.GetAllAsync(
                   expression: x => x.UserId == id ,
                   include: query => query.Include(x => x.User)
                                           .Include(x => x.Product)
                                           .ThenInclude(p => p.Category)
                                           .Include(x => x.Product)
                                           .ThenInclude(p => p.Brand)
                                           .Include(d => d.ComboProduct)
                                           .ThenInclude(co => co.Combo)
                                           .Include(d => d.ComboProduct)
                                           .ThenInclude(co => co.Product)
                                           .ThenInclude(p => p.Category)
                                           .Include(d => d.ComboProduct)
                                           .ThenInclude(co => co.Product)
                                           .ThenInclude(p => p.Brand)
                );
                var data = dataQuery;
                var wishlist = dataQuery.FirstOrDefault();
                if (wishlist == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/wishlist/{id}", null, wishlist);
                    return new ResponseObject(404, "Wishlist not found.", wishlist);
                }
                var result = new CartWishlistDTO
                {
                   WishlistId = wishlist.WishlistId,
                   Users = new UserMapperDTO
                   {
                       UserId = wishlist.UserId,
                       UserName = wishlist.User.UserName,
                       FullName = wishlist.User.FullName,
                       Email = wishlist.User.Email,
                       Avatar = wishlist.User.Avatar,
                       Address = wishlist.User.Address,
                       Gender = wishlist.User.Gender,
                       PlaceOfBirth = wishlist.User.PlaceOfBirth,
                       PhoneNumber = wishlist.User.PhoneNumber,
                       DateOfBirth = wishlist.User.DateOfBirth.HasValue ? wishlist.User.DateOfBirth.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                       Nationality = wishlist.User.Nationality,
                   },
                    Products = data.Where(x => x.ProductId != null) // Only filter for items with a non-null ProductId
                    .Select(p => new ProductDTO
                    {
                        ProductId = (int)p.ProductId,
                        Name = p.Product.Name,
                        Slug = p.Product.Slug,
                        Image = p.Product.Image,
                        Price = p.Product.Price,
                        Discount = p.Product.Discount,
                        IsActive = p.Product.IsActive,
                        BrandId = p.Product.BrandId,
                        BrandName = p.Product.Brand.Name,
                        Description = p.Product.Description,
                        CreateDate = p.Product.CreateDate.HasValue ? p.Product.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        UpdateBy = p.Product.UpdateBy,
                        UpdateDate = p.Product.UpdateDate.HasValue ? p.Product.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteBy = p.Product.DeleteBy,
                        DeleteDate = p.Product.DeleteDate.HasValue ? p.Product.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteFlag = p.Product.DeleteFlag,
                    }).ToList(),
                    Combos = data.Where(x => x.ComboProductId != null) // Only filter for items with a non-null ProductId
                    .Select(c => new ComboDTO
                    {
                        ComboId = c.ComboProduct.Combo.ComboId,
                        Name = c.ComboProduct.Combo.Name,
                        Image = c.ComboProduct.Combo.Image,
                        Price = c.ComboProduct.Combo.Price,
                        IsActive = c.ComboProduct.Combo.IsActive,
                        Description = c.ComboProduct.Combo.Description,
                        CreateBy = c.ComboProduct.Combo.CreateBy,
                        CreateDate = c.ComboProduct.Combo.CreateDate.HasValue ? c.ComboProduct.Combo.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        UpdateBy = c.ComboProduct.Combo.UpdateBy,
                        UpdateDate = c.ComboProduct.Combo.UpdateDate.HasValue ? c.ComboProduct.Combo.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteBy = c.ComboProduct.Combo.DeleteBy,
                        DeleteDate = c.ComboProduct.Combo.DeleteDate.HasValue ? c.ComboProduct.Combo.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        DeleteFlag = c.ComboProduct.Combo.DeleteFlag,
                        Products = data
                        .Where(d => d.ComboProduct?.Product != null)
                        .Where(d => d.ComboProduct.ProductId == c.ComboProduct.ProductId)
                        .Select(d => new ProductDTO
                        {
                            ProductId = d.ComboProduct.Product.ProductId,
                            Name = d.ComboProduct.Product.Name,
                            Slug = d.ComboProduct.Product.Slug,
                            Image = d.ComboProduct.Product.Image,
                            Price = d.ComboProduct.Product.Price,
                            Discount = d.ComboProduct.Product.Discount,
                            IsActive = d.ComboProduct.Product.IsActive,
                            Description = d.ComboProduct.Product.Description,
                            BrandId = d.ComboProduct.Product.BrandId,
                            BrandName = d.ComboProduct.Product.Brand.Name,
                            CategoryId = d.ComboProduct.Product.CategoryId,
                            CategoryName = d.ComboProduct.Product.Category.Name,
                            CreateBy = d.ComboProduct.Product.CreateBy,
                            CreateDate = d.ComboProduct.Product.CreateDate.HasValue ? d.ComboProduct.Product.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            UpdateBy = d.ComboProduct.Product.UpdateBy,
                            UpdateDate = d.ComboProduct.Product.UpdateDate.HasValue ? d.ComboProduct.Product.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteBy = d.ComboProduct.Product.DeleteBy,
                            DeleteDate = d.ComboProduct.Product.DeleteDate.HasValue ? d.ComboProduct.Product.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteFlag = d.ComboProduct.Product.DeleteFlag,
                        }).ToList()
                    }).ToList()
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/wishlist/{id}", null, result);
                    return new ResponseObject(404, "Wishlist not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", "/api/wishlist/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/wishlist/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(WishlistModel model)
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

                // check item wishlist
                var checkWishlist = await _unitOfWork.WishlistRepository.GetAllAsync(
                    x => x.UserId == model.UserId && (x.ProductId == model.ProductId || x.ComboProductId == model.ComboId));
                if(checkWishlist.Count() > 0)
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/wishlist", null, null);
                    return new ResponseObject(404, "Product or combo already exists in favorites.", null);
                }


                // mapper data
                var wishlist = new Wishlist();
                var userCheck = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
                if (userCheck != null)
                {
                    wishlist.UserId = model.UserId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/wishlist", null, null);
                    return new ResponseObject(404, "User not found.", null);
                }

                if (model.ProductId != null) 
                {
                    var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync((int)model.ProductId);
                    if (proCheck != null)
                    {
                        wishlist.ProductId = model.ProductId;
                    }
                    else
                    {
                        LogHelper.LogWarning(_logger, "POST", "/api/wishlist", null, null);
                        return new ResponseObject(404, "Product not found.", null);
                    }
                }
                if (model.ComboId != null) 
                {
                    var comboCheck = await _unitOfWork.ComboProductRepository.GetByIdAsync((int)model.ComboId);
                    if (comboCheck != null)
                    {
                        wishlist.ComboProductId = model.ComboId;
                    }
                    else
                    {
                        LogHelper.LogWarning(_logger, "POST", "/api/wishlist", null, null);
                        return new ResponseObject(404, "Product not found.", null);
                    }
                }
                wishlist.CreateBy = Constant.SYSADMIN;
                wishlist.CreateDate = DateTime.Now;
                await _unitOfWork.WishlistRepository.SaveOrUpdateAsync(wishlist);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/wishlist", model, wishlist);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/wishlist", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/wishlist/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var wishlist = await _unitOfWork.WishlistRepository.GetByIdAsync(id);
                if (wishlist == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/wishlist/{id}", id, "Wishlist not found.");
                    return new ResponseObject(404, "Wishlist not found.", null);
                }

                await _unitOfWork.WishlistRepository.DeleteAsync(wishlist);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/wishlist/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/wishlist/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
