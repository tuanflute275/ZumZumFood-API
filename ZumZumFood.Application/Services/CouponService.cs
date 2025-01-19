namespace ZumZumFood.Application.Services
{
    public class CouponService : ICouponService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponService> _logger;
        public CouponService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CouponService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.CouponRepository.GetAllAsync(
                    expression: x => x.DeleteFlag != true && string.IsNullOrEmpty(keyword) 
                    || x.Code.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.CouponId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.CouponId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.Code);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.Code);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.CouponId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<CouponDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/coupon", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/coupon");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.CouponRepository.GetAllAsync(
                   expression: x => x.CouponId == id && x.DeleteFlag != true,
                   include: query => query.Include(x => x.CouponConditions)
                );
                if (dataQuery == null || !(dataQuery.Count() > 0))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon/{id}", null, dataQuery.FirstOrDefault());
                    return new ResponseObject(404, "Coupon not found.", dataQuery.FirstOrDefault());
                }
                var coupon = dataQuery.FirstOrDefault();
                var result = new CouponMapperDTO
                {
                    CouponId = coupon.CouponId,
                    Code = coupon.Code,
                    Description = coupon.Description,
                    IsActive = coupon.IsActive,
                    CreateBy = coupon.CreateBy,
                    CreateDate = coupon.CreateDate.HasValue ? coupon.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    UpdateBy = coupon.UpdateBy,
                    UpdateDate = coupon.UpdateDate.HasValue ? coupon.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteBy = coupon.DeleteBy,
                    DeleteDate = coupon.DeleteDate.HasValue ? coupon.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null,
                    DeleteFlag = coupon.DeleteFlag,
                    Conditions = coupon.CouponConditions.Select(c => new CouponConditionDTO
                    {
                        Attribute = c.Attribute,
                        Operator = c.Operator,
                        Value = c.Value,
                        DiscountAmount = c.DiscountAmount,
                    }).ToList()
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon/{id}", null, result);
                    return new ResponseObject(404, "Coupon not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/coupon/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/parameter/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(CouponModel model)
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
                var coupon = new Coupon();
                coupon.Code = model.Code;
                coupon.Description = model.Description;
                coupon.IsActive = model.IsActive;
                coupon.Scope = model.Scope;
                coupon.ScopeId = model.ScopeId;
                coupon.CreateBy = model.CreateBy;
                coupon.CreateDate = DateTime.Now;
                await _unitOfWork.CouponRepository.SaveOrUpdateAsync(coupon);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/coupon", model, coupon);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/coupon", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, CouponModel model)
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
                var coupon = await _unitOfWork.CouponRepository.GetByIdAsync(id);
                if (coupon == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/coupon", null, $"Coupon not found with id {id}");
                    return new ResponseObject(400, $"Coupon not found with id {id}", null);
                }
                coupon.Code = model.Code;
                coupon.Description = model.Description;
                coupon.IsActive = model.IsActive;
                coupon.Scope = model.Scope;
                coupon.ScopeId = model.ScopeId;
                coupon.UpdateBy = model.UpdateBy;
                coupon.UpdateDate = DateTime.Now;
                await _unitOfWork.CouponRepository.SaveOrUpdateAsync(coupon);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/coupon", model, coupon);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/coupon", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var coupon = await _unitOfWork.CouponRepository.GetByIdAsync(id);
                if (coupon == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/coupon/{id}", id, "Coupon not found.");
                    return new ResponseObject(404, "Coupon not found.", null);
                }
                await _unitOfWork.CouponRepository.DeleteAsync(coupon);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/coupon/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/coupon/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> CalculateCouponValueAsync(string couponCode, double? totalAmount, 
            string? currentCategory, int? currentQuantity, string? currentUserType, 
            string? currentPaymentMethod, string? currentBrand, int? currentOrderCount)
        {
            var dataQuery = await _unitOfWork.CouponRepository.GetAllAsync(x => x.Code.Contains(couponCode));
            var coupon = dataQuery.FirstOrDefault();
            if (coupon == null)
            {
                return new ResponseObject(400, "Coupon not found", totalAmount);
            }

            if (!coupon.IsActive)
            {
                return new ResponseObject(400, "Coupon is not active", totalAmount);
            }
            double discount = await CalculateDiscountAsync(coupon, (double)totalAmount, currentCategory, (int)currentQuantity,
                currentUserType, currentPaymentMethod, currentBrand, (int)currentOrderCount);
            double finalAmount = (double)totalAmount - discount;
            return new ResponseObject(200, "Caculate successfully", finalAmount);
        }

        private async Task<double> CalculateDiscountAsync(Coupon coupon, double totalAmount, 
            string currentCategory, int currentQuantity, string currentUserType, 
            string currentPaymentMethod, string currentBrand, int currentOrderCount)
        {
            var conditions = await _unitOfWork.CouponConditionRepository.GetAllAsync(x => x.CouponId == coupon.CouponId);
            double discount = 0.0;
            double updatedTotalAmount = totalAmount;
            foreach (var condition in conditions)
            {
                // EAV (Entity - Attribute - Value)
                string attribute = condition.Attribute;
                string _operator = condition.Operator;
                string value = condition.Value;
                double percentDiscount = Convert.ToDouble(condition.DiscountAmount);

                // Điều kiện "minimum_amount"
                if (totalAmount > 0 && attribute.Equals("minimum_amount", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals(">") && updatedTotalAmount > Convert.ToDouble(value))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                } 
                // Điều kiện "applicable_date"
                else if (attribute.Equals("applicable_date", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals("BETWEEN", StringComparison.OrdinalIgnoreCase))
                    {
                        var dateRange = value.Split('|');
                        DateTime startDate = DateTime.Parse(dateRange[0]);
                        DateTime endDate = DateTime.Parse(dateRange[1]);
                        DateTime currentDate = DateTime.Now;

                        if (currentDate >= startDate && currentDate <= endDate)
                        {
                            discount += updatedTotalAmount * percentDiscount / 100;
                        }
                    }
                }
                // Điều kiện "category"
                else if (currentCategory != null && attribute.Equals("category", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals("=") && currentCategory.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                }
                // Điều kiện "quantity"
                else if (currentQuantity > 0 && attribute.Equals("quantity", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals(">=") && currentQuantity >= Convert.ToInt32(value))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                }
                // Điều kiện "user_type"
                else if (currentUserType != null && attribute.Equals("user_type", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals("=") && currentUserType.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                }
                // Điều kiện "payment_method"
                else if (currentPaymentMethod != null && attribute.Equals("payment_method", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals("=") && currentPaymentMethod.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                }
                // Điều kiện "brand"
                else if (currentBrand != null && attribute.Equals("brand", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals("=") && currentBrand.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                }
                // Điều kiện "order_count"
                else if (currentOrderCount > 0 && attribute.Equals("order_count", StringComparison.OrdinalIgnoreCase))
                {
                    if (_operator.Equals("=") && currentOrderCount == Convert.ToInt32(value))
                    {
                        discount += updatedTotalAmount * percentDiscount / 100;
                    }
                }
                
                // Các điều kiện khác có thể bổ sung tại đây
                updatedTotalAmount = updatedTotalAmount - discount;
            }

            return discount;
        }
    }
}
