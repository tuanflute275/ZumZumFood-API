using ZumZumFood.Application.Utils.Common;
using ZumZumFood.Application.Utils.Helpers;

namespace ZumZumFood.Application.Services
{
    public class BannerService : IBannerService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BannerService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BannerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BannerService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.ValidateInput(keyword, sort, pageNo);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.BannerRepository.GetAllAsync(
                    expression: s => s.DeleteFlag != true && string.IsNullOrEmpty(keyword) || s.Title.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.BannerId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.BannerId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.Title);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.Title);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.BannerId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<BannerDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/banner", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/banner");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.BannerRepository.GetAllAsync(
                   expression: x => x.BannerId == id && x.DeleteFlag != true
                );
                if (dataQuery == null || !(dataQuery.Count() > 0))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner/{id}", null, dataQuery.FirstOrDefault());
                    return new ResponseObject(404, "Banner not found.", dataQuery.FirstOrDefault());
                }
                var result = _mapper.Map<BannerDTO>(dataQuery.FirstOrDefault());

                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner/{id}", null, result);
                    return new ResponseObject(404, "Banner not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/banner/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/banner/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(BannerModel model)
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

                if(model.ImageFile == null)
                {
                    return new ResponseObject(400, "Validation error: ImageFile is required.", null);
                }
                // end validate

                // mapper data
                var banner = new Banner();
                banner.Title = model.Title;
                banner.IsActive = model.IsActive;
                banner.CreateBy = model.CreateBy;
                banner.CreateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "banners");
                    banner.Image = image;
                }

                await _unitOfWork.BannerRepository.SaveOrUpdateAsync(banner);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/banner", model, banner);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/banner", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, BannerModel model)
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
                var banner = await _unitOfWork.BannerRepository.GetByIdAsync(id);
                if(banner == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/banner", null, $"Banner not found with id {id}");
                    return new ResponseObject(400, $"Banner not found with id {id}", null);
                }
                banner.Title = model.Title;
                banner.IsActive = model.IsActive;
                banner.UpdateBy = model.UpdateBy;
                banner.UpdateDate = DateTime.Now;
                if (model.ImageFile != null)
                {
                    var image = await FileUploadHelper.UploadImageAsync(model.ImageFile, model.OldImage, request.Scheme, request.Host.Value, "banners");
                    banner.Image = image;
                }

                await _unitOfWork.BannerRepository.SaveOrUpdateAsync(banner);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/banner", model, banner);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/banner", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var banner = await _unitOfWork.BannerRepository.GetByIdAsync(id);
                if (banner == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/banner/{id}", id, "Banner not found.");
                    return new ResponseObject(404, "Banner not found.", null);
                }
                banner.DeleteFlag = true;
                banner.DeleteBy = deleteBy;
                banner.DeleteDate = DateTime.Now;
                await _unitOfWork.BannerRepository.SaveOrUpdateAsync(banner);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/banner/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/banner/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.BannerRepository.GetAllAsync(x => x.DeleteFlag == true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted categories found." });
                    return new ResponseObject(404, "No deleted categories found.", null);
                }
                var data = _mapper.Map<List<BannerDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/banner/deleted-data", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var banner = await _unitOfWork.BannerRepository.GetByIdAsync(id);
                if (banner == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/banner/{id}/restore", new { id }, new { message = "Banner not found." });
                    return new ResponseObject(404, "Banner not found.", null);
                }

                if ((bool)!banner.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/banner/{id}/restore", new { id }, new { message = "Banner is not flagged as deleted." });
                    return new ResponseObject(400, "Banner is not flagged as deleted.", null);
                }

                banner.DeleteFlag = false;
                banner.DeleteBy = null;
                banner.DeleteDate = null;

                await _unitOfWork.BannerRepository.SaveOrUpdateAsync(banner);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/banner/{id}/restore", id, "Banner restored successfully");
                return new ResponseObject(200, "Banner restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/banner/{id}/restore", id);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/banner/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var banner = await _unitOfWork.BannerRepository.GetByIdAsync(id);
                if (banner == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/banner/{id}", id, "Banner not found.");
                    return new ResponseObject(404, "Banner not found.", null);
                }

                await _unitOfWork.BannerRepository.DeleteAsync(banner);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/banner/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/banner/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
