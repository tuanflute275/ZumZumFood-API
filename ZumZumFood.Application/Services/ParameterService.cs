using ZumZumFood.Application.Models.Queries.Components;

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

        public async Task<ResponseObject> GetAllPaginationAsync(ParameterQuery parameterQuery)
        {
            var limit = parameterQuery.PageSize > 0 ? parameterQuery.PageSize : int.MaxValue;
            var start = parameterQuery.PageNo > 0 ? (parameterQuery.PageNo - 1) * limit : 0;
            try
            {
                var dataQuery = _unitOfWork.ParameterRepository.GetAllAsync(
                    expression: s => s.DeleteFlag != true && string.IsNullOrEmpty(parameterQuery.Keyword) 
                    || s.ParaScope.Contains(parameterQuery.Keyword) || s.ParaName.Contains(parameterQuery.Keyword) 
                    || s.ParaType.Contains(parameterQuery.Keyword)
                );
                var query = await dataQuery;

                // Áp dụng sắp xếp
                if (!string.IsNullOrEmpty(parameterQuery.SortColumn))
                {
                    query = parameterQuery.SortColumn switch
                    {
                        "Name" when parameterQuery.SortAscending => query.OrderBy(x => x.ParaName),
                        "Name" when !parameterQuery.SortAscending => query.OrderByDescending(x => x.ParaName),
                        "Id" when parameterQuery.SortAscending => query.OrderBy(x => x.ParameterId),
                        "Id" when !parameterQuery.SortAscending => query.OrderByDescending(x => x.ParameterId),
                        _ => query
                    };
                }
                else
                {
                    // Sắp xếp mặc định
                    query = query.OrderByDescending(x => x.ParameterId);
                }

                // Get total count
                var totalCount = query.Count();

                // Apply pagination if SelectAll is false
                var pagedQuery = parameterQuery.SelectAll
                    ? query.ToList()
                    : query
                        .Skip(start)
                        .Take(limit)
                        .ToList();

                // Map to DTOs
                var data = _mapper.Map<List<ParameterDTO>>(pagedQuery);

                // Prepare response
                var responseData = new
                {
                    items = data,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / parameterQuery.PageSize) > 0 ? (int)Math.Ceiling((double)totalCount / parameterQuery.PageSize) : 0,
                    pageNumber = parameterQuery.PageNo,
                    pageSize = parameterQuery.PageSize
                };

                LogHelper.LogInformation(_logger, "GET", "/api/parameter", $"Query: {JsonConvert.SerializeObject(parameterQuery)}", data.Count);
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
                   expression: x => x.ParameterId == id && x.DeleteFlag != true
                );
                if (dataQuery == null || !(dataQuery.Count() > 0))
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter/{id}", null, dataQuery.FirstOrDefault());
                    return new ResponseObject(404, "Parameter not found.", dataQuery.FirstOrDefault());
                }
                var result = _mapper.Map<ParameterDTO>(dataQuery.FirstOrDefault());
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/parameter/{id}", null, result);
                    return new ResponseObject(404, "Parameter not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/parameter/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/parameter/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveAsync(ParameterModel model)
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
                parameter.CreateBy = model.CreateBy;
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

        public async Task<ResponseObject> UpdateAsync(int id, ParameterModel model)
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
                if (parameter == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/parameter", null, $"Parameter not found with id {id}");
                    return new ResponseObject(400, $"Parameter not found with id {id}", null);
                }
                parameter.ParaScope = model.ParaScope;
                parameter.ParaName = model.ParaName;
                parameter.ParaType = model.ParaType;
                parameter.ParaDesc = model.ParaDesc;
                parameter.ParaShortValue = model.ParaShortValue;
                parameter.ParaLobValue = model.ParaLobValue;
                parameter.AdminAccessibleFlag = model.AdminAccessibleFlag;
                parameter.UserAccessibleFlag = model.UserAccessibleFlag;
                parameter.UpdateBy = model.UpdateBy;
                parameter.UpdateDate = DateTime.Now;
                await _unitOfWork.ParameterRepository.SaveOrUpdateAsync(parameter);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/parameter", model, parameter);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/parameter", model);
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
                parameter.DeleteBy = deleteBy;
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
