using ZumZumFood.Application.Utils.Common;
using ZumZumFood.Application.Utils.Helpers;

namespace ZumZumFood.Application.Services
{
    public class LogService : ILogService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LogService> _logger;
        public LogService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LogService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/log", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.LogRepository.GetAllAsync(
                    expression: x => x.TimeActionRequest != null 
                                        && x.TimeLogin == null
                                        && string.IsNullOrEmpty(keyword) 
                                        || x.UserName.Contains(keyword) 
                                        || x.WorkTation.Contains(keyword) 
                                        || x.IpAdress.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` 
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.LogId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.LogId);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.LogId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<LogDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/log", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/log");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetAllUserLoginPaginationAsync(string? keyword, string? sort, int pageNo = 1)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.ValidateInput(keyword, sort, pageNo);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/log", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.LogRepository.GetAllAsync(
                    expression: x => x.TimeActionRequest == null
                                        && x.TimeLogin != null
                                        || x.TimeLogout != null
                                        && string.IsNullOrEmpty(keyword)
                                        || x.UserName.Contains(keyword)
                                        || x.WorkTation.Contains(keyword)
                                        || x.IpAdress.Contains(keyword)
                );
                var query = await dataQuery;

                // Apply dynamic sorting based on the `sort` 
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.LogId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.LogId);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.LogId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<LogDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/log", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/log");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/log/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.LogRepository.GetAllAsync(
                   expression: x => x.LogId == id
                );
                if (dataQuery == null || !(dataQuery.Count() > 0))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/log/{id}", null, dataQuery.FirstOrDefault());
                    return new ResponseObject(404, "Log not found.", dataQuery.FirstOrDefault());
                }
                var result = _mapper.Map<LogDTO>(dataQuery.FirstOrDefault());
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/log/{id}", null, result);
                    return new ResponseObject(404, "Log not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/log/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/log/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(LogModel model)
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
                var log = new Log();
                log.UserName = model.UserName;
                log.WorkTation = model.WorkTation;
                log.Request = model.Request;
                log.Response = model.Response;
                log.IpAdress = model.IpAdress;
                if (model.IsLogin)
                    log.TimeLogin = DateTime.Now;
                else
                    log.TimeActionRequest = DateTime.Now;
                
                await _unitOfWork.LogRepository.SaveOrUpdateAsync(log);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/log", model, log);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/log", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id)
        {
            try
            {
                // mapper data
                var log = await _unitOfWork.LogRepository.GetByIdAsync(id);
                if (log == null && log.TimeActionRequest == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/log", null, $"Log not found with id {id}");
                    return new ResponseObject(400, $"Log not found with id {id}", null);
                }
                log.TimeLogout = DateTime.Now;
                await _unitOfWork.LogRepository.SaveOrUpdateAsync(log);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/log", log, log);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/log", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/log/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var log = await _unitOfWork.LogRepository.GetByIdAsync(id);
                if (log == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/log/{id}", id, "Log not found.");
                    return new ResponseObject(404, "Log not found.", null);
                }
                await _unitOfWork.LogRepository.DeleteAsync(log);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/log/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/log/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
