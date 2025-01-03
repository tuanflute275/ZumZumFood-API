namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/log")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _logService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _logService.GetByIdAsync(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] LogRequestModel model)
        {
            return await _logService.SaveAsync(model);
        }

        [HttpGet("user")]
        public async Task<ResponseObject> FindUserLoginAll(string? keyword, string? sort, int page = 1)
        {
            return await _logService.GetAllUserLoginPaginationAsync(keyword, sort, page);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("user/{id}")]
        public async Task<ResponseObject> UpdateUserLogin(int id)
        {
            return await _logService.UpdateAsync(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _logService.DeleteAsync(id);
        }
    }
}
