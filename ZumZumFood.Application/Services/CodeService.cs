using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Application.Services
{
    public class CodeService : ICodeService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CodeService> _logger;
        public CodeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CodeService> logger)
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
                    LogHelper.LogWarning(_logger, "GET", $"/api/code", null, "Input contains invalid special characters");
                    return new ResponseObject(400, "Input contains invalid special characters", validationResult);
                }
                var dataQuery = _unitOfWork.CodeRepository.GetAllAsync(
                    expression: s => s.DeleteFlag != true && string.IsNullOrEmpty(keyword) || s.CodeDes.Contains(keyword)
                );
                var query = await dataQuery;
               
                // Apply dynamic sorting based on the `sort` parameter
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "Id-ASC":
                            query = query.OrderBy(x => x.CodeId);
                            break;
                        case "Id-DESC":
                            query = query.OrderByDescending(x => x.CodeId);
                            break;
                        case "Name-ASC":
                            query = query.OrderBy(x => x.CodeDes);
                            break;
                        case "Name-DESC":
                            query = query.OrderByDescending(x => x.CodeDes);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.CodeId);
                            break;
                    }
                }

                // Map data to dataDTO
                var dataList = query.ToList();
                var data = _mapper.Map<List<CodeDTO>>(dataList);

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
                LogHelper.LogInformation(_logger, "GET", "/api/code", null, pagedData.Count());
                return new ResponseObject(200, "Query data successfully", responseData);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/code");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetByCodeIdAsync(string codeId)
        {
            try
            {
                var dataQuery = await _unitOfWork.CodeRepository.GetAllAsync(
                   expression: x => x.CodeId == codeId && x.DeleteFlag != true
                );
                var code = dataQuery.FirstOrDefault();
                if (code == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/code/{codeId}", null, code);
                    return new ResponseObject(404, "Code not found.", code);
                }
                var result = _mapper.Map<CodeDTO>(code);
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/code/{codeId}", null, result);
                    return new ResponseObject(404, "Code not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/code/{codeId}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/code/{codeId}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveCodeAsync(CodeModel model)
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
                // check dupplicate
                var checkData = await _unitOfWork.CodeRepository.GetByCodeIdAsync(model.CodeId);
                if(checkData != null)
                {
                    LogHelper.LogInformation(_logger, "POST", "/api/code", model, null);
                    return new ResponseObject(400, "Data dupplicate", null);
                }
                // end validate

                // mapper data
                var code = new Code();
                code.CodeId = model.CodeId;
                code.CodeDes = model.CodeDes;
                code.CreateDate = DateTime.Now;
                await _unitOfWork.CodeRepository.SaveAsync(code);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/code", model, code);
                return new ResponseObject(200, "Create data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/code", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateCodeAsync(string codeId, CodeUpdateModel model)
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
                var code = await _unitOfWork.CodeRepository.GetByCodeIdAsync(codeId);
                if (code == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/code", null, $"Code not found with codeId {codeId}");
                    return new ResponseObject(400, $"Code not found with codeId {codeId}", null);
                }

                code.CodeDes = model.CodeDes;
                code.UpdateDate = DateTime.Now;
                await _unitOfWork.CodeRepository.UpdateAsync(code);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/code", model, code);
                return new ResponseObject(200, "Update data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/code", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
      
        public async Task<ResponseObject> DeleteCodeAsync(string codeId)
        {
            try
            {
                var code = await _unitOfWork.CodeRepository.GetByCodeIdAsync(codeId);
                if (code == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/code/{codeId}", codeId, "Code not found.");
                    return new ResponseObject(404, "Code not found.", null);
                }
                
                await _unitOfWork.CodeRepository.DeleteAsync(code);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/code/{codeId}", codeId, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/code/{codeId}", codeId);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        // code Values
        public async Task<ResponseObject> GetListCodeValueByCodeId(string codeId)
        {
            try
            {
                var dataQuery = _unitOfWork.CodeValueRepository.GetAllAsync(
                    expression: x => x.CodeId == codeId
                );
                var query = await dataQuery;

                // Map data to dataDTO
                var dataList = query.ToList();

                if (dataList == null || dataList.Count() == 0)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/code-value/{codeId}", null, null);
                    return new ResponseObject(404, "Code value not found.", null);
                }

                var data = _mapper.Map<List<CodeValueDTO>>(dataList);

                LogHelper.LogInformation(_logger, "GET", "/api/code-value", null, data.Count());
                return new ResponseObject(200, "Query data successfully", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/code-value");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> GetByCodeIdAndCodeValueAsync(string codeId, string codeValue)
        {
            try
            {
                var dataQuery = await _unitOfWork.CodeValueRepository.GetAllAsync(
                   expression: x => x.CodeId == codeId && x.CodeValue == codeValue && x.DeleteFlag != true
                );
                var value = dataQuery.FirstOrDefault();
                if (value == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/code-value/{codeId}", null, null);
                    return new ResponseObject(404, "Code value not found.", null);
                }
                var result = _mapper.Map<CodeValueDTO>(value);
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/code-value/{codeId}", null, result);
                    return new ResponseObject(404, "Code value not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", $"/api/code-value/{codeId}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/code-value/{codeId}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> SaveCodeValueAsync(CodeValueModel model)
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
                var value = new CodeValues();
                model.CopyProperties(value);
                value.CreateDate = DateTime.Now;
                await _unitOfWork.CodeValueRepository.SaveAsync(value);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/code-value", model, value);
                return new ResponseObject(200, "Create data successfully", value);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/code-value", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> UpdateCodeValueAsync(string codeId, string codeValue, CodeValueUpdateModel model)
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
                var value = await _unitOfWork.CodeValueRepository.GetByCodeIdAndCodeValueAsync(codeId, codeValue);
                if (value == null)
                {
                    LogHelper.LogWarning(_logger, "PUT", $"/api/code-value", null, $"Code value not found with codeId {codeId}");
                    return new ResponseObject(400, $"Code value not found with codeId {codeId}", null);
                }
                model.CopyProperties(value);
                value.UpdateDate = DateTime.Now;
                await _unitOfWork.CodeValueRepository.UpdateAsync(value);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "PUT", "/api/code-value", model, value);
                return new ResponseObject(200, "Update data successfully", value);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "PUT", $"/api/code-value", model);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public async Task<ResponseObject> DeleteCodeValueAsync(string codeId, string codeValue)
        {
            try
            {
                var value = await _unitOfWork.CodeValueRepository.GetByCodeIdAndCodeValueAsync(codeId, codeValue);
                if (value == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/code-value/{codeId}", codeId, "Code value not found.");
                    return new ResponseObject(404, "Code value not found.", null);
                }

                await _unitOfWork.CodeValueRepository.DeleteAsync(value);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/code-value/{codeId}", codeId, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/code-value/{codeId}", codeId);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        // location

        public async Task<ResponseObject> GetLocation(int parentId)
        {
            try
            {
                var dataQuery = _unitOfWork.LocationRepository.GetAllAsync(
                    expression: x => x.ParentId == parentId
                );
                var query = await dataQuery;

                // Map data to dataDTO
                var dataList = query.ToList();

                if (dataList == null || dataList.Count() == 0)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/code/location/{parentId}", null, null);
                    return new ResponseObject(404, "Location not found.", null);
                }

                var data = _mapper.Map<List<LocationDTO>>(dataList);

                LogHelper.LogInformation(_logger, "GET", "/api/code/location", null, data.Count());
                return new ResponseObject(200, "Query data successfully", data);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/code/location");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
