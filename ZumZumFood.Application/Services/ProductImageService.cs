using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Application.Services
{
    public class ProductImageService : IProductImageService
    {
        IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductImageService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductImageService(IUnitOfWork unitOfWork, ILogger<ProductImageService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseObject> GetByIdAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/product-image/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.ProductImageRepository.GetAllAsync(
                   expression: x => x.ProductImageId == id
                );
                var productImage = dataQuery.FirstOrDefault();
                if (productImage == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/product-image/{id}", null, productImage);
                    return new ResponseObject(404, "Product Image not found.", productImage);
                }
                var result = new ProductImageDTO
                {
                    ProductImageId = productImage.ProductId,
                    Path = productImage.Path,
                    
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/product-image/{id}", null, result);
                    return new ResponseObject(404, "Product Image not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", "/api/product-image/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/product-image/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(ProductImageModel model)
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

                var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
                if (proCheck == null)
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product-image", null, null);
                    return new ResponseObject(404, "Product not found.", null);
                }
                // end validate

                // mapper data
                foreach (var item in model.ImageFiles)
                {
                    var proImage = new ProductImage();
                    proImage.ProductId = model.ProductId;
                    proImage.CreateBy = Constant.SYSADMIN;
                    proImage.CreateDate = DateTime.Now;
                    var image = await FileUploadHelper.UploadImageAsync(item, null, request.Scheme, request.Host.Value, "productImages");
                    proImage.Path = image;
                    await _unitOfWork.ProductImageRepository.SaveOrUpdateAsync(proImage);
                }
                   
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/product-image", model, null);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/product-image", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, ProductImageUpdateModel model)
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
                var proImage = await _unitOfWork.ProductImageRepository.GetByIdAsync(id);
                if (proImage == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/product-image", null, $"Product image not found with id {id}");
                    return new ResponseObject(400, $"Product image not found with id {id}", null);
                }

                proImage.UpdateBy = Constant.SYSADMIN;
                proImage.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, null, request.Scheme, request.Host.Value, "categories");
                    proImage.Path = image;
                }

                await _unitOfWork.ProductImageRepository.SaveOrUpdateAsync(proImage);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/product-image", model, proImage);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/product-image", model);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/product-image/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var proImage = await _unitOfWork.ProductImageRepository.GetByIdAsync(id);
                if (proImage == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/product-image/{id}", id, "Product image not found.");
                    return new ResponseObject(404, "Product image not found.", null);
                }
                await _unitOfWork.ProductImageRepository.DeleteAsync(proImage);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/product-image/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/product-image/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
