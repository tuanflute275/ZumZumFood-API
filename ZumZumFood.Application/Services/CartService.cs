using StackExchange.Redis;
using ZumZumFood.Application.Utils.Common;
using ZumZumFood.Application.Utils.Helpers;
using ZumZumFood.Domain.Entities;

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
                        LogHelper.LogWarning(_logger, "POST", "/api/cart", null, null);
                        return new ResponseObject(404, "User not found.", null);
                    }
                    if (model.ProductId != null)
                    {
                        var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync((int)model.ProductId);
                        if (proCheck != null)
                        {
                            cart.Quantity = model.Quantity;
                            cart.ProductId = model.ProductId;
                            cart.TotalAmount = model.Quantity * (proCheck.Discount > 0 ? proCheck.Discount : proCheck.Price);
                        }
                        else
                        {
                            LogHelper.LogWarning(_logger, "POST", "/api/cart", null, null);
                            return new ResponseObject(404, "Product not found.", null);
                        }
                    }
                    if (model.ComboId != null)
                    {
                        var dataQuery = await _unitOfWork.ComboProductRepository.GetAllAsync(
                             expression: x => x.ComboId == model.ComboId,
                            include: query => query.Include(x => x.Combo).Include(x => x.Product)
                            );
                        var comboCheck = dataQuery.FirstOrDefault();
                        if (comboCheck != null)
                        {
                            cart.ComboProductId = model.ComboId;
                            cart.Quantity = model.Quantity;
                            cart.TotalAmount = model.Quantity * comboCheck.Combo.Price;
                        }
                        else
                        {
                            LogHelper.LogWarning(_logger, "POST", "/api/cart", null, null);
                            return new ResponseObject(404, "Product not found.", null);
                        }
                    }
                    await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                }
                else
                {
                    var cart = cartCheck.FirstOrDefault();
                    cart.Quantity++;
                    cart.ProductId = model.ProductId;
                    if(model.ProductId != null)
                    {
                        var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync((int)model.ProductId);
                        var amountTemp = model.Quantity * (proCheck.Discount > 0 ? proCheck.Discount : proCheck.Price);
                        cart.TotalAmount += amountTemp;
                    }

                    if (model.ComboId != null)
                    {
                        var dataQuery = await _unitOfWork.ComboProductRepository.GetAllAsync(
                            expression: x => x.ComboId == model.ComboId,
                           include: query => query.Include(x => x.Combo)
                           );
                        var comboCheck = dataQuery.FirstOrDefault();
                        var amountTemp = model.Quantity * (comboCheck.Combo.Price);
                        cart.TotalAmount += amountTemp;
                    }
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

                if(cart.ProductId != null && cart.ProductId > 0)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)cart.ProductId);
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
                        cart.TotalAmount += (product.Discount > 0 ? product.Discount : product.Price);
                        await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                    }
                }

                if (cart.ComboProductId != null && cart.ComboProductId > 0)
                {
                    var dataQuery = await _unitOfWork.ComboProductRepository.GetAllAsync(
                        expression: x => x.ComboId == cart.ComboProductId,
                        include: query => query.Include(x => x.Combo)
                    );
                    var combo = dataQuery.FirstOrDefault();
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
                            cart.TotalAmount -= combo.Combo.Price;
                            await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                        }
                    }
                    else
                    {
                        cart.Quantity++;
                        cart.TotalAmount += combo.Combo.Price;
                        await _unitOfWork.CartRepository.SaveOrUpdateAsync(cart);
                    }
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
