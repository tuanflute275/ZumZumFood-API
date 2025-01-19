namespace ZumZumFood.Application.Services
{
    public class CouponConditionService : ICouponConditionService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponConditionService> _logger;
        public CouponConditionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CouponConditionService> logger)
        {
            _logger = logger;
            _mapper = mapper;
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon-condition/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.CouponConditionRepository.GetAllAsync(
                   expression: x => x.CouponId == id
                );
                if (dataQuery == null || !(dataQuery.Count() > 0))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon-condition/{id}", null, null);
                    return new ResponseObject(404, "Coupon condition not found.", null);
                }
                var result = _mapper.Map<List<CouponConditionDTO>>(dataQuery);
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon-condition/{id}", null, result);
                    return new ResponseObject(404, "Coupon condition not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/coupon-condition/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/coupon-condition/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(CouponConditionModel model)
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
                var condition = new CouponCondition();
                var couponCheck = await _unitOfWork.CouponRepository.GetByIdAsync(model.CouponId);
                if(couponCheck != null)
                {
                    condition.CouponId = model.CouponId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/coupon-condition", null, null);
                    return new ResponseObject(404, "Coupon not found.", null);
                }
                condition.Attribute = model.Attribute;
                condition.Operator = model.Operator;
                condition.Value = model.Value;
                condition.DiscountAmount = model.DiscountAmount;
                await _unitOfWork.CouponConditionRepository.SaveOrUpdateAsync(condition);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/coupon-condition", model, condition);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/coupon-condition", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, CouponConditionModel model)
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
                var condition = await _unitOfWork.CouponConditionRepository.GetByIdAsync(id);
                if (condition == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/coupon-condition", null, $"Coupon condition not found with id {id}");
                    return new ResponseObject(400, $"Coupon condition not found with id {id}", null);
                }

                var couponCheck = await _unitOfWork.CouponRepository.GetByIdAsync(model.CouponId);
                if (couponCheck != null)
                {
                    condition.CouponId = model.CouponId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/coupon-condition", null, null);
                    return new ResponseObject(404, "Coupon not found.", null);
                }
                condition.Attribute = model.Attribute;
                condition.Operator = model.Operator;
                condition.Value = model.Value;
                condition.DiscountAmount = model.DiscountAmount;
                await _unitOfWork.CouponConditionRepository.SaveOrUpdateAsync(condition);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/coupon-condition", model, condition);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/coupon-condition", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/coupon-condition/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var condition = await _unitOfWork.CouponConditionRepository.GetByIdAsync(id);
                if (condition == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/coupon-condition/{id}", id, "Coupon condition not found.");
                    return new ResponseObject(404, "Coupon condition not found.", null);
                }
                await _unitOfWork.CouponConditionRepository.DeleteAsync(condition);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/coupon-condition/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/coupon-condition/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
