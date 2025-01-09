namespace ZumZumFood.Application.Services
{
    public class CartService : ICartService
    {
        IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;
        public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/cart/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.CartRepository.GetAllAsync(
                   expression: x => x.UserId == id,
                   include: query => query.Include(x => x.User)
                                           .Include(x => x.Product)
                                           .ThenInclude(p => p.Category)
                                           .Include(x => x.Product)
                                           .ThenInclude(p => p.Brand)
                );
                var data = dataQuery;
                var cart = dataQuery.FirstOrDefault();
                if (cart == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/cart/{id}", null, cart);
                    return new ResponseObject(404, "Cart not found.", cart);
                }
                var result = new CartWishlistDTO
                {
                    WishlistId = cart.CartId,
                    Users = new UserMapperDTO
                    {
                        UserId = cart.UserId,
                        UserName = cart.User.UserName,
                        FullName = cart.User.FullName,
                        Email = cart.User.Email,
                        Avatar = cart.User.Avatar,
                        Address = cart.User.Address,
                        Gender = cart.User.Gender,
                        PlaceOfBirth = cart.User.PlaceOfBirth,
                        PhoneNumber = cart.User.PhoneNumber,
                        DateOfBirth = cart.User.DateOfBirth.HasValue ? cart.User.DateOfBirth.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        Nationality = cart.User.Nationality,
                    },
                    Products = data.Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
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
                    }).ToList()
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/cart/{id}", null, result);
                    return new ResponseObject(404, "Cart not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/cart/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/cart/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(CartModel model)
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

                var cartCheck = await _unitOfWork.CartRepository.GetAllAsync(x => x.UserId == model.UserId && x.ProductId == model.ProductId);
                if(cartCheck.Count() == 0 || cartCheck == null || cartCheck.FirstOrDefault() == null)
                {
                    var cart = new Cart();
                    var userCheck = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
                    if (userCheck != null)
                    {
                        cart.UserId = model.UserId;
                    }
                    else
                    {
                        LogHelper.LogWarning(_logger, "POST", "/api/product-comment", null, null);
                        return new ResponseObject(404, "User not found.", null);
                    }
                    var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
                    if (proCheck != null)
                    {
                        cart.ProductId = model.ProductId;
                        cart.Quantity = model.Quantity;
                        cart.TotalAmount = model.Quantity * (proCheck.Discount > 0 ? proCheck.Discount : proCheck.Price);
                    }
                    else
                    {
                        LogHelper.LogWarning(_logger, "POST", "/api/product-comment", null, null);
                        return new ResponseObject(404, "Product not found.", null);
                    }
                    await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                }
                else
                {
                    var cart = cartCheck.FirstOrDefault();
                    cart.Quantity++;
                    var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
                    cart.ProductId = model.ProductId;
                    var amountTemp = model.Quantity * (proCheck.Discount > 0 ? proCheck.Discount : proCheck.Price);
                    cart.TotalAmount += amountTemp;
                    await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                }

                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/cart", model, null);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/cart", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, string type)
        {
            try
            {
                if (!(type.Contains(Constant.CART_UPDATE_MINUS) || type.Contains(Constant.CART_UPDATE_PLUS)))
                {
                    return new ResponseObject(400, "Invalid Cart type value. Only 'minus' or 'plus' are allowed.", null);
                }
                var cart = await _unitOfWork.CartRepository.GetByIdAsync(id);
                if (cart == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/cart", null, $"Cart not found with id {id}");
                    return new ResponseObject(400, $"Cart not found with id {id}", null);
                }

                var product = await _unitOfWork.ProductRepository.GetByIdAsync(cart.ProductId);
                if (type.Contains(Constant.CART_UPDATE_MINUS))
                {
                    cart.Quantity--;
                    if (cart.Quantity == 0)
                    {
                        LogHelper.LogInformation(_logger, "DELETE", $"/api/cart", null, $"Cart deleted with id {id} as quantity reached 0");
                        await _unitOfWork.CartRepository.DeleteAsync(cart);
                    }
                    else
                    {
                        cart.TotalAmount -= (product.Discount > 0 ? product.Discount : product.Price);
                        await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                    }
                }
                else
                {
                    cart.Quantity++;
                    cart.TotalAmount+= (product.Discount > 0 ? product.Discount : product.Price);
                    await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                }
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/cart", null, cart);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/cart", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/cart/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var cart = await _unitOfWork.CartRepository.GetByIdAsync(id);
                if (cart == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/cart/{id}", id, "Cart not found.");
                    return new ResponseObject(404, "Cart not found.", null);
                }
                await _unitOfWork.CartRepository.DeleteAsync(cart);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/cart/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/cart/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
