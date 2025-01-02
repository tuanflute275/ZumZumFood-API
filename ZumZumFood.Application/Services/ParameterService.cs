namespace ZumZumFood.Application.Services
{
    public class ParameterService : IParameterService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ParameterService> _logger;
        public ParameterService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ParameterService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.ParameterRepository.GetAllAsync(
                    expression: s => s.DeleteFlag == false && string.IsNullOrEmpty(keyword) 
                    || s.ParaScope.Contains(keyword) || s.ParaName.Contains(keyword) || s.ParaType.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.ParameterId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.ParameterId);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.ParameterId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<ParameterDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/parameter", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/parameter");
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.ParameterRepository.GetAllAsync(
                   expression: x => x.ParameterId == id && x.DeleteFlag == false
                );
                var result = _mapper.Map<ParameterDTO>(dataQuery.FirstOrDefault());
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/parameter/{id}", null, result);
                    return new ResponseObject(404, "Parameter not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", "/api/parameter/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/parameter/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(ParameterRequestModel model)
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
                var parameter = new Parameter();
                parameter = _mapper.Map<Parameter>(model);
                parameter.CreateBy = Constant.SYSADMIN;
                parameter.CreateDate = DateTime.Now;
                await _unitOfWork.ParameterRepository.SaveOrUpdateAsync(parameter);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/parameter", model, parameter);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/parameter", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateAsync(int id, ParameterRequestModel model)
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
                var parameter = await _unitOfWork.ParameterRepository.GetByIdAsync(id);
                parameter.ParaScope = model.ParaScope;
                parameter.ParaName = model.ParaName;
                parameter.ParaType = model.ParaType;
                parameter.ParaDesc = model.ParaDesc;
                parameter.ParaShortValue = model.ParaShortValue;
                parameter.ParaLobValue = model.ParaLobValue;
                parameter.AdminAccessibleFlag = model.AdminAccessibleFlag;
                parameter.UserAccessibleFlag = model.UserAccessibleFlag;
                parameter.UpdateBy = Constant.SYSADMIN;
                parameter.UpdateDate = DateTime.Now;
                await _unitOfWork.ParameterRepository.SaveOrUpdateAsync(parameter);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/parameter", model, parameter);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/parameter", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteFlagAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var parameter = await _unitOfWork.ParameterRepository.GetByIdAsync(id);
                if (parameter == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/parameter/{id}", id, "Parameter not found.");
                    return new ResponseObject(404, "Parameter not found.", null);
                }
                parameter.DeleteFlag = true;
                parameter.DeleteBy = Constant.SYSADMIN;
                parameter.DeleteDate = DateTime.Now;
                await _unitOfWork.ParameterRepository.SaveOrUpdateAsync(parameter);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", $"/api/parameter/{id}", id, "Deleted Flag successfully");
                return new ResponseObject(200, "Delete Flag data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/parameter/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetDeletedListAsync()
        {
            try
            {
                var deletedData = await _unitOfWork.ParameterRepository.GetAllAsync(x => x.DeleteFlag == true);
                if (deletedData == null || !deletedData.Any())
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/deleted-data", null, new { message = "No deleted categories found." });
                    return new ResponseObject(404, "No deleted categories found.", null);
                }
                var data = _mapper.Map<List<ParameterDTO>>(deletedData);
                LogHelper.LogInformation(_logger, "GET", $"/api/deleted-data", null, "Query data deleted successfully");
                return new ResponseObject(200, "Query data delete flag successfully.", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", "/api/parameter/deleted-data", null);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var parameter = await _unitOfWork.ParameterRepository.GetByIdAsync(id);
                if (parameter == null)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/parameter/{id}/restore", new { id }, new { message = "Parameter not found." });
                    return new ResponseObject(404, "Parameter not found.", null);
                }

                if ((bool)!parameter.DeleteFlag)
                {
                    LogHelper.LogWarning(_logger, "POST", $"/api/parameter/{id}/restore", new { id }, new { message = "Parameter is not flagged as deleted." });
                    return new ResponseObject(400, "Parameter is not flagged as deleted.", null);
                }

                parameter.DeleteFlag = false;
                parameter.DeleteBy = null;
                parameter.DeleteDate = null;

                await _unitOfWork.ParameterRepository.SaveOrUpdateAsync(parameter);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", $"/api/parameter/{id}/restore", id, "Parameter restored successfully");
                return new ResponseObject(200, "Parameter restored successfully.", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/parameter/{id}/restore", id);
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var parameter = await _unitOfWork.ParameterRepository.GetByIdAsync(id);
                if (parameter == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/parameter/{id}", id, "Parameter not found.");
                    return new ResponseObject(404, "Parameter not found.", null);
                }
                await _unitOfWork.ParameterRepository.DeleteAsync(parameter);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/parameter/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/parameter/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
