namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/code")]
    public class CodeController : ControllerBase
    {
        private readonly ICodeService _codeService;
        public CodeController(ICodeService codeService)
        {
            _codeService = codeService; 
        }

        [HttpPost("search")]
        public async Task<ResponseObject> FindAll(CodeQuery codeQuery)
        {
            return await _codeService.GetAllPaginationAsync(codeQuery);
        }

        [HttpGet("{codeId}")]
        public async Task<ResponseObject> FindByCodeId(string codeId)
        {
            return await _codeService.GetByCodeIdAsync(codeId);
        }

        [HttpPost]
        public async Task<ResponseObject> SaveCode(CodeModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _codeService.SaveCodeAsync(model);
        }

        [HttpPut("{codeId}")]
        public async Task<ResponseObject> UpdateCode(string codeId, CodeUpdateModel model)
        {
            var user = HttpContext.User;
            model.UpdateBy = TokenHelper.GetCurrentUsername(user);
            return await _codeService.UpdateCodeAsync(codeId, model);
        }

        [HttpDelete("{codeId}")]
        public async Task<ResponseObject> DeleteCode(string codeId)
        {
            return await _codeService.DeleteCodeAsync(codeId);
        }

        // code Values
        [HttpGet("values")]
        public async Task<ResponseObject> GetListCodeValueByCodeId(string codeId)
        {
            return await _codeService.GetListCodeValueByCodeId(codeId);
        }

        [HttpGet("values/{codeId}/{codeValue}")]
        public async Task<ResponseObject> FindOneByCodeIdAndCodeValue(string codeId, string codeValue)
        {
            return await _codeService.GetByCodeIdAndCodeValueAsync(codeId, codeValue);
        }

        [HttpPost("values")]
        public async Task<ResponseObject> SaveCodeValue(CodeValueModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _codeService.SaveCodeValueAsync(model);
        }

        [HttpPut("values/{codeId}/{codeValue}")]
        public async Task<ResponseObject> UpdateCodeValue(string codeId, string codeValue, CodeValueUpdateModel model)
        {
            var user = HttpContext.User;
            model.UpdateBy = TokenHelper.GetCurrentUsername(user);
            return await _codeService.UpdateCodeValueAsync(codeId,codeValue, model);
        }

        [HttpDelete("values/{codeId}/{codeValue}")]
        public async Task<ResponseObject> DeleteCodeValue(string codeId, string codeValue)
        {
            return await _codeService.DeleteCodeValueAsync(codeId, codeValue);
        }

        // location
        [HttpGet("location")]
        public async Task<ResponseObject> GetLocation(int parentId)
        {
            return await _codeService.GetLocation(parentId);
        }
    }
}
