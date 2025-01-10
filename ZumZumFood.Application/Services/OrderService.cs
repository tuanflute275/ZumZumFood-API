namespace ZumZumFood.Application.Services
{
    public class OrderService : IOrderService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.ValidateInput(keyword, sort, pageNo);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/order", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }

                var dataQuery = _unitOfWork.OrderRepository.GetAllAsync(
                    expression: x => string.IsNullOrEmpty(keyword) || x.OrderFullName.Contains(keyword)
                    || x.OrderAddress.Contains(keyword) || x.OrderPhoneNumber.Contains(keyword)
                );
                var query = await dataQuery;

                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.OrderId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.OrderId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.OrderFullName);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.OrderFullName);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.OrderId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<OrderDTO>>(dataList);

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

                LogHelper.LogInformation(_logger, "GET", "/api/order", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/order");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetByIdAsync(int id)
        {
            try
            {
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/order/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }

                var dataQuery = await _unitOfWork.OrderRepository.GetAllAsync(
                   expression: x => x.OrderId == id,
                   include: query => query.Include(x => x.User)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(d => d.Product)
                                          .ThenInclude(p => p.Brand)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(d => d.Product)
                                          .ThenInclude(p => p.Category)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(d => d.ComboProduct)
                                          .ThenInclude(co => co.Combo)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(d => d.ComboProduct)
                                          .ThenInclude(co => co.Product)
                                          .ThenInclude(p => p.Brand)
                                          .Include(x => x.OrderDetails)
                                          .ThenInclude(d => d.ComboProduct)
                                          .ThenInclude(co => co.Product)
                                          .ThenInclude(p => p.Category)
                );

                var order = dataQuery.FirstOrDefault();
                if (order == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/order/{id}", null, null);
                    return new ResponseObject(404, "Order not found.", null);
                }

                var result = new OrderMapperDTO
                {
                    OrderId = order.OrderId,
                    OrderQuantity = order.OrderQuantity,
                    OrderAmount = order.OrderAmount,
                    OrderFullName = order.OrderFullName,
                    OrderAddress = order.OrderAddress,
                    OrderPhoneNumber = order.OrderPhoneNumber,
                    OrderEmail = order.OrderEmail,
                    OrderPaymentMethods = order.OrderPaymentMethods,
                    OrderStatusPayment = order.OrderStatusPayment,
                    OrderStatus = order.OrderStatus,
                    OrderNote = order.OrderNote,
                    OrderDate = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    Users = new UserMapperDTO
                    {
                        UserId = order.UserId,
                        UserName = order.User.UserName,
                        FullName = order.User.FullName,
                        Avatar = order.User.Avatar,
                        Email = order.User.Email,
                        PhoneNumber = order.User.PhoneNumber,
                        Address = order.User.Address,
                        Gender = order.User.Gender,
                        DateOfBirth = order.User.DateOfBirth.HasValue ? order.User.DateOfBirth.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                        PlaceOfBirth = order.User.PlaceOfBirth,
                        Nationality = order.User.Nationality
                    },
                    Details = order.OrderDetails.Select(detail => new OrderDetailDTO
                    {
                        OrderDetailId = detail.OrderDetailId,
                        Quantity = detail.Quantity,
                        TotalMoney = detail.TotalMoney,
                        OrderDetailType = detail.OrderDetailType,
                        Product = detail.Product != null ? new ProductDTO
                        {
                            ProductId = detail.Product.ProductId,
                            Name = detail.Product.Name,
                            Slug = detail.Product.Slug,
                            Image = detail.Product.Image,
                            Price = detail.Product.Price,
                            Discount = detail.Product.Discount,
                            IsActive = detail.Product.IsActive,
                            Description = detail.Product.Description,
                            BrandId = detail.Product.BrandId,
                            BrandName = detail.Product.Brand.Name,
                            CategoryId = detail.Product.CategoryId,
                            CategoryName = detail.Product.Category.Name,
                            CreateBy = detail.Product.CreateBy,
                            CreateDate = detail.Product.CreateDate.HasValue ? detail.Product.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            UpdateBy = detail.Product.UpdateBy,
                            UpdateDate = detail.Product.UpdateDate.HasValue ? detail.Product.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteBy = detail.Product.DeleteBy,
                            DeleteDate = detail.Product.DeleteDate.HasValue ? detail.Product.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteFlag = detail.Product.DeleteFlag,
                        } : null,
                        Combo = detail.ComboProduct != null ? new ComboDTO
                        {
                            ComboId = detail.ComboProduct.Combo.ComboId,
                            Name = detail.ComboProduct.Combo.Name,
                            Image = detail.ComboProduct.Combo.Image,
                            Price = detail.ComboProduct.Combo.Price,
                            IsActive = detail.ComboProduct.Combo.IsActive,
                            Description = detail.ComboProduct.Combo.Description,
                            CreateBy = detail.ComboProduct.Combo.CreateBy,
                            CreateDate = detail.ComboProduct.Combo.CreateDate.HasValue ? detail.ComboProduct.Combo.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            UpdateBy = detail.ComboProduct.Combo.UpdateBy,
                            UpdateDate = detail.ComboProduct.Combo.UpdateDate.HasValue ? detail.ComboProduct.Combo.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteBy = detail.ComboProduct.Combo.DeleteBy,
                            DeleteDate = detail.ComboProduct.Combo.DeleteDate.HasValue ? detail.ComboProduct.Combo.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                            DeleteFlag = detail.ComboProduct.Combo.DeleteFlag,
                            Products = order.OrderDetails.Where(d => d.ComboProduct?.Product != null).Select(d => new ProductDTO
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
                        } : null
                    }).ToList()
                };

                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/order/{id}", null, result);
                    return new ResponseObject(404, "Order not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/order/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/order/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(OrderModel model)
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

                // save order
                var order = new Domain.Entities.Order();
                order.UserId = model.UserId;
                order.OrderFullName = model.OrderFullName;
                order.OrderPhoneNumber = model.OrderPhoneNumber;
                order.OrderAddress = model.OrderAddress;
                order.OrderEmail = model.OrderEmail;
                order.OrderQuantity = model.OrderQuantity;
                order.OrderAmount = model.OrderAmount;
                order.OrderNote = model.OrderNote;
                order.OrderPaymentMethods = model.OrderPaymentMethods;
                order.OrderStatusPayment = model.OrderStatusPayment;
                order.OrderStatus = Constant.DEFAULT_STATUS_ORDER;
                await _unitOfWork.OrderRepository.SaveOrUpdateAsync(order);

                // save order detail
                var orderId = order.OrderId;
                order.OrderAmount = 1;
                order.OrderQuantity = 1;
                var carts = await _unitOfWork.CartRepository.GetAllAsync(x => x.UserId == model.UserId);
                foreach (var item in carts)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.OrderId = orderId;
                    detail.Quantity = item.Quantity;
                    if(item.ProductId != null)
                    {
                        detail.ProductId = item.ProductId;
                        var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync((int)item.ProductId);
                        detail.TotalMoney = Convert.ToDouble(item.Quantity * (proCheck.Discount > 0 ? proCheck.Discount : proCheck.Price));
                    }
                    if (item.ComboProductId != null) 
                    {
                        detail.ComboProductId = item.ComboProductId;
                        var dataQuery = await _unitOfWork.ComboProductRepository.GetAllAsync(
                            expression: x => x.ComboId == item.ComboProductId,
                           include: query => query.Include(x => x.Combo)
                           );
                        var comboCheck = dataQuery.FirstOrDefault();
                        detail.TotalMoney = item.Quantity * comboCheck.Combo.Price;
                    }
                    await _unitOfWork.OrderDetailRepository.SaveOrUpdateAsync(detail);
                }

                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/order", model, order);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/order", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, OrderUpdateModel model)
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
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/order", null, $"Order not found with id {id}");
                    return new ResponseObject(400, $"Order not found with id {id}", null);
                }

                order.OrderStatus = model.Status;
                await _unitOfWork.OrderRepository.SaveOrUpdateAsync(order);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/order", model, order);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/order", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/order/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }

                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
                // Bắt đầu: Xóa các phụ thuộc khóa ngoại
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetAllAsync(x => x.OrderId == id);
                if (orderDetail != null && orderDetail.Any())
                {
                    await _unitOfWork.OrderDetailRepository.DeleteRangeAsync(orderDetail.ToList());
                    await _unitOfWork.SaveChangeAsync();
                    // Kết thúc: Xóa các phụ thuộc khóa ngoại
                }

                if (order == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/order/{id}", id, "Order not found.");
                    return new ResponseObject(404, "Order not found.", null);
                }
                await _unitOfWork.OrderRepository.DeleteAsync(order);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/order/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/order/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
