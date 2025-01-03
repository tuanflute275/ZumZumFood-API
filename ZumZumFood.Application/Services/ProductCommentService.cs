using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Application.Services
{
    public class ProductCommentService : IProductCommentService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCommentService> _logger;
        public ProductCommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductCommentService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/productComment/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.ProductCommentRepository.GetAllAsync(
                   expression: x => x.ProductCommentId == id,
                   include: query => query.Include(x => x.User)
                );
                var comment = dataQuery.FirstOrDefault();
                if (comment == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/product-comment/{id}", null, comment);
                    return new ResponseObject(404, "Comment not found.", comment);
                }
                var result = new ProductCommentDTO
                {
                    Email = comment.Email,
                    Message = comment.Message,
                    Name = comment.Name,
                    Users = new UserProductDTO
                    {
                        UserId = comment.UserId,
                        UserName = comment.User.UserName,
                        UserFullName = comment.User.FullName,
                        UserEmail = comment.User.Email,
                        DateOfBirth = comment.User.DateOfBirth,
                        Nationality = comment.User.Nationality,
                        PlaceOfBirth = comment.User.PlaceOfBirth,
                        UserAddress = comment.User.Address,
                        UserAvatar = comment.User.Avatar,
                        UserGender = comment.User.Gender,
                        UserPhoneNumber = comment.User.PhoneNumber
                    }
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/product-comment/{id}", null, result);
                    return new ResponseObject(404, "Comment not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", "/api/product-comment/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/product-comment/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(ProductCommentRequestModel model)
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
                var comment = new ProductComment();
                var proCheck = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
                if (proCheck != null)
                {
                    comment.ProductId = model.ProductId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product-comment", null, null);
                    return new ResponseObject(404, "Product not found.", null);
                }
                var userCheck = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
                if (userCheck != null)
                {
                    comment.UserId = model.UserId;
                }
                else
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/product-comment", null, null);
                    return new ResponseObject(404, "User not found.", null);
                }
                comment.Email = model.Email;
                comment.Name = model.Name;
                comment.Message = model.Message;
                await _unitOfWork.ProductCommentRepository.SaveOrUpdateAsync(comment);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/product-comment", model, comment);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/product-comment", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, ProductCommentUpdateRequestModel model)
        {
            try
            {
                // mapper data
                var comment = await _unitOfWork.ProductCommentRepository.GetByIdAsync(id);
                if (comment == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/product-comment", null, $"Comment not found with id {id}");
                    return new ResponseObject(400, $"Comment not found with id {id}", null);
                }
                comment.Message = model.Message;
                await _unitOfWork.ProductCommentRepository.SaveOrUpdateAsync(comment);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/product-comment", model, comment);
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
                var comment = await _unitOfWork.ProductCommentRepository.GetByIdAsync(id);
                if (comment == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/product-comment/{id}", id, "Comment not found.");
                    return new ResponseObject(404, "Comment not found.", null);
                }
                await _unitOfWork.ProductCommentRepository.DeleteAsync(comment);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/product-comment/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/product-comment/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
