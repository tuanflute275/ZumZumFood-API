namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/combo")]
    public class ComboController : ControllerBase
    {
        private readonly IComboService _comboService;
        public ComboController(IComboService comboService)
        {
            _comboService = comboService;
        }

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _comboService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _comboService.GetByIdAsync(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ResponseObject> Save([FromForm] ComboModel model)
        {
            return await _comboService.SaveAsync(model);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromForm] ComboModel model)
        {
            return await _comboService.UpdateAsync(id, model);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            return await _comboService.DeleteFlagAsync(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("deleted-data")]
        public async Task<ResponseObject> GetDeletedUsers()
        {
            return await _comboService.GetDeletedListAsync();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("restore/{id}")]
        public async Task<ResponseObject> RestoreUser(int id)
        {
            return await _comboService.RestoreAsync(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _comboService.DeleteAsync(id);
        }
    }
}
