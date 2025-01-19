namespace ZumZumFood.Application.Services
{
    public class ProductDetailService : IProductDetailService
    {
        IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductDetailService> _logger;
        public ProductDetailService(IUnitOfWork unitOfWork, ILogger<ProductDetailService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product-detail/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.ProductDetailRepository.GetAllAsync(
                   expression: x => x.ProductDetailId == id
                );
                var detail = dataQuery.FirstOrDefault();
                if (detail == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/product-detail/{id}", null, detail);
                    return new ResponseObject(404, "Product detail not found.", detail);
                }
                var result = new ProductDetailDTO();
                detail.CopyProperties(result);

                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/product-detail/{id}", null, result);
                    return new ResponseObject(404, "Product detail not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/product-detail/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/product-detail/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(ProductDetailModel model)
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
                var detail = new ProductDetail();
                var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
                if (proCheck != null)
                {
                    detail.ProductId = model.ProductId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product-comment", null, null);
                    return new ResponseObject(404, "Product not found.", null);
                }
                // mapper data (các tên giống nhau sẽ tự map vào nhau)
                model.CopyProperties(detail);
                detail.CreateBy = model.CreateBy;
                detail.CreateDate = DateTime.Now;
                await _unitOfWork.ProductDetailRepository.SaveOrUpdateAsync(detail);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/product-detail", model, detail);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/product-detail", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, ProductDetailModel model)
        {
            try
            {
                // mapper data
                var detail = await _unitOfWork.ProductDetailRepository.GetByIdAsync(id);
                if (detail == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/product-detail", null, $"Product detail not found with id {id}");
                    return new ResponseObject(400, $"Comment not found with id {id}", null);
                }

                // mapper data (các tên giống nhau sẽ tự map vào nhau)
                model.CopyProperties(detail);
                detail.UpdateDate = DateTime.Now;
                await _unitOfWork.ProductDetailRepository.SaveOrUpdateAsync(detail);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/product-detail", model, detail);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/product-comment", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product-comment/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var detail = await _unitOfWork.ProductDetailRepository.GetByIdAsync(id);
                if (detail == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/product-detail/{id}", id, "Product detail not found.");
                    return new ResponseObject(404, "Product Detail not found.", null);
                }
                await _unitOfWork.ProductDetailRepository.DeleteAsync(detail);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/product-detail/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/product-detail/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
